using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyConcordiaID.Models.Event
{
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseEntities _database;

        public EventRepository(DatabaseEntities context)
        {
            _database = context;
        }

        /// <summary>
        ///  Get a list of current scheduled events which have not expired 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventInformation> GetActiveEvents()
        {
            var today = DateTime.UtcNow;

            var activeEvents = _database.EVENTS
                .Where(e => e.TIME_END > today)
                .Select(e => new EventInformation
                {
                    EventId = e.ID_PK,
                    Name = e.NAME,
                    Description = e.DESCRIPTION,
                    Location = e.LOCATION,
                    Room = e.ROOM,
                    TimeBegin = e.TIME_BEGIN,
                    TimeEnd = e.TIME_END,
                    Type = e.TYPE,
                    Status = e.STATUS
                })
                .OrderByDescending(e => e.TimeBegin);

            return activeEvents;
        }

        /// <summary>
        /// Get a list of all events where the user is part of the organization team (creator, mod, scanner) 
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetAdminEvents(string netname)
        {

            var user = _database.STUDENTS
                .AsNoTracking()
                .Where(u => u.NETNAME == netname)
                .FirstOrDefault();

            if (user != null)
            {
                var attendee = Role.Attendee.ToString();
                var today = DateTime.UtcNow;

                var studentEvents = _database.EVENT_USERS
                    .Where(e => e.STUDENT_NETNAME_FK == netname && e.ROLE != attendee)
                    .Select(e => new AvailableEvent
                    {
                        UserRole = e.ROLE,
                        Information = new EventInformation
                        {
                            EventId = e.EVENT.ID_PK,
                            Name = e.EVENT.NAME,
                            Description = e.EVENT.DESCRIPTION,
                            Location = e.EVENT.LOCATION,
                            Room = e.EVENT.ROOM,
                            TimeBegin = e.EVENT.TIME_BEGIN,
                            TimeEnd = e.EVENT.TIME_END,
                            Type = e.EVENT.TYPE,
                            Status = e.EVENT.STATUS
                        }
                    })
                    .ToList();

                //Sort list of dates closest to current date
                studentEvents = studentEvents
                    .OrderBy(n => (today - n.Information.TimeBegin).Duration())
                    .ThenBy(n => (today - n.Information.TimeEnd).Duration())
                    .ToList();

                return studentEvents;
            }
            return null;

        }

        /// <summary>
        ///  Get all the open events that user can participate and all those that he is registered to or is part of the organization team
        /// </summary>
        /// <param name="netname"></param>
        /// <returns>List of events</returns>
        public IEnumerable<AvailableEvent> GetAttendeeEvents(string netname)
        {
            var today = DateTime.UtcNow;
            var cancelled = EventStatusType.Cancelled.ToString();
            var attendee = Role.Attendee.ToString();


            // event can't be canceled or expired 
            var events = _database.EVENT_USERS
                .Where(e => e.STUDENT_NETNAME_FK == netname && e.EVENT.STATUS != cancelled && e.EVENT.TIME_END > today)
                .Select(e => new AvailableEvent
                {
                    UserRole = e.ROLE,
                    Information = new EventInformation
                    {
                        EventId = e.EVENT.ID_PK,
                        Name = e.EVENT.NAME,
                        Description = e.EVENT.DESCRIPTION,
                        Location = e.EVENT.LOCATION,
                        Room = e.EVENT.ROOM,
                        TimeBegin = e.EVENT.TIME_BEGIN,
                        TimeEnd = e.EVENT.TIME_END,
                        Type = e.EVENT.TYPE,
                        Status = e.EVENT.STATUS
                    }
                })
                .ToList();

            var open = EventType.Open.ToString();


            // event must be open & not canceled or expired
            var openEvents = _database.EVENTS
                .Where(e => e.TYPE == open && e.STATUS != cancelled && e.TIME_END > today)
                .Select(e => new AvailableEvent
                {
                    UserRole = attendee,
                    Information = new EventInformation
                    {
                        EventId = e.ID_PK,
                        Name = e.NAME,
                        Description = e.DESCRIPTION,
                        Location = e.LOCATION,
                        Room = e.ROOM,
                        TimeBegin = e.TIME_BEGIN,
                        TimeEnd = e.TIME_END,
                        Type = e.TYPE,
                        Status = e.STATUS
                    }
                })
                .ToList();


            //merge list
            // remove duplicate : if you created a open event, you will be a creator thus part of the Event_users
            //                    When we get all available open event you will have an duplicate "attendee object" 
            //Sort list of dates closest to current date
            events = events
               .Concat(openEvents)
               .GroupBy(x => x.Information.EventId)
               .Select(s => s.First())
               .OrderBy(n => (today - n.Information.TimeBegin).Duration())
               .ThenBy(n => (today - n.Information.TimeEnd).Duration())
               .ToList();

            return events;

        }

        /// <summary>
        ///  Get specific event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public EventInformation GetEventById(string eventId)
        {

            var currentEvent = _database.EVENTS
                .Where(e => e.ID_PK == eventId)
                .Select(e => new EventInformation
                {
                    EventId = e.ID_PK,
                    Name = e.NAME,
                    Description = e.DESCRIPTION,
                    Location = e.LOCATION,
                    Room = e.ROOM,
                    TimeBegin = e.TIME_BEGIN,
                    TimeEnd = e.TIME_END,
                    Type = e.TYPE,
                    Status = e.STATUS
                })
                .FirstOrDefault();

            return currentEvent;
        }

        /// <summary>
        ///  Get a list of all the events
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventInformation> GetEvents()
        {
            var events = _database.EVENTS
                .Select(e => new EventInformation
                {
                    EventId = e.ID_PK,
                    Name = e.NAME,
                    Description = e.DESCRIPTION,
                    Location = e.LOCATION,
                    Room = e.ROOM,
                    TimeBegin = e.TIME_BEGIN,
                    TimeEnd = e.TIME_END,
                    Type = e.TYPE,
                    Status = e.STATUS
                })
                .OrderByDescending(e => e.TimeBegin);

            return events;
        }

        /// <summary>
        ///  Get a list of all events that have this specifc status 
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public IEnumerable<EventInformation> GetEventsByStatus(EventStatusType eventStatus)
        {
            var status = eventStatus.ToString();

            var events = _database.EVENTS
                .Where(e => e.STATUS == status)
                .Select(e => new EventInformation
                {
                    EventId = e.ID_PK,
                    Name = e.NAME,
                    Description = e.DESCRIPTION,
                    Location = e.LOCATION,
                    Room = e.ROOM,
                    TimeBegin = e.TIME_BEGIN,
                    TimeEnd = e.TIME_END,
                    Type = e.TYPE,
                    Status = e.STATUS
                })
                .OrderByDescending(e => e.TimeBegin);

            return events;

        }

        /// <summary>
        ///  Find all the users going to a specific event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="orderUserOnTop"></param>
        /// <param name="netName"></param>
        /// <returns></returns>
        public IEnumerable<EventUserInformation> GetEventUsers(string eventId, bool orderUserOnTop, string netName)
        {

            var selectedEvent = _database.EVENTS
                .Where(e => e.ID_PK == eventId)
                .FirstOrDefault();

            if (selectedEvent != null)
            {
                var eventUsers = _database.EVENT_USERS
                    .Where(user => user.EVENT_ID == eventId)
                    .Select(u => new EventUserInformation
                    {
                        Id = u.ID_PK,
                        Role = u.ROLE,
                        Status = u.STATUS,
                        StudentAccount = new StudentBasicInformation
                        {
                            Id = u.STUDENT.ID,
                            NetName = u.STUDENT.NETNAME,
                            FirstName = u.STUDENT.FIRSTNAME,
                            LastName = u.STUDENT.LASTNAME
                        }
                    })
                    .ToList();

                if (orderUserOnTop)
                {
                    //authenticated user will be the first user of the list
                    eventUsers = eventUsers
                        .OrderByDescending(order => order.StudentAccount.NetName == netName)
                        .ToList();
                }

                return eventUsers;
            }

            return null;
        }



        /// <summary>
        ///  Add a new event with his creator 
        /// </summary>
        /// <param name="information"></param>
        /// <param name="netname"></param>
        public void InsertEvent(NewEvent information, string netname)
        {

            var newEvent = new EVENT
            {
                NAME = information.Name,
                DESCRIPTION = information.Description,
                TIME_BEGIN = information.TimeBegin,
                TIME_END = information.TimeEnd,
                LOCATION = information.Location,
                ROOM = information.Room,
                TYPE = information.Type.ToString(),
                STATUS = EventStatusType.Scheduled.ToString()
            };

            _database.EVENTS.Add(newEvent);
            _database.SaveChanges();


            var newOwner = new EVENT_USERS
            {
                STUDENT_NETNAME_FK = netname,
                ROLE = Role.Creator.ToString(),
                STATUS = UserStatus.EventOrganizer.ToString(),
                EVENT_ID = newEvent.ID_PK
            };

            _database.EVENT_USERS.Add(newOwner);
            _database.SaveChanges();


            //TODO: will need to catch save error using logs

        }

        /// <summary>
        ///  Add a user to an event if he exist in the system 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public EventActionResult InsertUser(NewEventUser user)
        {

            var selectedEvent = _database.EVENTS
                .Where(e => e.ID_PK == user.EventID)
                .FirstOrDefault();

            if (selectedEvent != null)
            {
                var netName = "";
                if (!string.IsNullOrEmpty(user.UserNetname))
                {
                    var student = _database.STUDENTS
                        .Where(s => s.NETNAME == user.UserNetname)
                        .FirstOrDefault();

                    if (student != null)
                    {
                        netName = student.NETNAME;
                    }
                }
                else if (user.UserId != null)
                {
                    var student = _database.STUDENTS
                        .Where(s => s.ID == user.UserId)
                        .FirstOrDefault();

                    if (student != null)
                    {
                        netName = student.NETNAME;
                    }

                }

                if (!string.IsNullOrEmpty(netName))
                {
                    //student does exist 


                    var userExist = _database.EVENT_USERS
                        .Where(u => u.EVENT_ID == user.EventID && u.STUDENT_NETNAME_FK == netName)
                        .FirstOrDefault();

                    if (userExist != null)
                    {
                        //user exist thus duplicate action
                        return EventActionResult.DuplicateUser;

                    }

                    var status = UserStatus.Tracking.ToString(); ;
                    if (string.Equals(selectedEvent.TYPE, EventType.Closed.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        status = UserStatus.Registered.ToString();
                    }

                    if (user.Role != Role.Attendee)
                    {
                        status = UserStatus.EventOrganizer.ToString();
                    }


                    var newUser = new EVENT_USERS
                    {
                        STUDENT_NETNAME_FK = netName,
                        ROLE = user.Role.ToString(),
                        STATUS = status,
                        EVENT_ID = user.EventID,
                    };

                    _database.EVENT_USERS.Add(newUser);
                    _database.SaveChanges();

                    return EventActionResult.Success;
                }
                else
                {
                    return EventActionResult.UserNotFound;
                }
            }
            else
            {
                return EventActionResult.EventNotFound;
            }
        }

        /// <summary>
        ///  Register user to a event 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ScannerResult RegisterScannedUser(ScannerUser user)
        {

            var processResult = new ScannerResult
            {
                Status = ScannerStatus.IdNotFound.ToString()
            };

            var currentUser = _database.STUDENTS
                .Where(e => e.ID == user.StudentId)
                .FirstOrDefault();

            if (currentUser != null)
            {
                //tracking user
                if (user.Type == EventType.Open)
                {
                    var newUser = new EVENT_USERS
                    {
                        STUDENT_NETNAME_FK = currentUser.NETNAME,
                        ROLE = Role.Attendee.ToString(),
                        STATUS = UserStatus.Tracking.ToString(),
                        EVENT_ID = user.EventID,
                    };

                    _database.EVENT_USERS.Add(newUser);
                    processResult.Status = ScannerStatus.Success.ToString();
                }
                else
                {
                    //validating his registation 
                    var registeredUser = _database.EVENT_USERS
                        .Where(r => r.EVENT_ID == user.EventID && r.STUDENT_NETNAME_FK == currentUser.NETNAME)
                        .FirstOrDefault();

                    if (registeredUser != null)
                    {
                        registeredUser.STATUS = UserStatus.Attending.ToString();
                        _database.SaveChanges();
                        processResult.Status = ScannerStatus.Success.ToString();

                    }
                    else
                    {
                        processResult.Status = ScannerStatus.UserWasNotRegistered.ToString();
                    }


                }


            }

            return processResult;

        }

        /// <summary>
        ///  Cancel an event 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public EventActionResult RemoveEvent(EventCancelled cancellation)
        {

            var removeEvent = _database.EVENTS
                .Where(e => e.ID_PK == cancellation.EventId)
                .FirstOrDefault();

            if (removeEvent != null)
            {
                //canceled event
                removeEvent.STATUS = EventStatusType.Cancelled.ToString();
                _database.SaveChanges();

                return EventActionResult.Success;
            }
            else
            {
                return EventActionResult.EventNotFound;
            }
        }

        /// <summary>
        /// Remove an user from a event
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Success/UserNotFound</returns>
        public EventActionResult RemoveUser(EventUser user)
        {
            var userToRemove = _database.EVENT_USERS
                .Where(u => u.ID_PK == user.UserId)
                .FirstOrDefault();

            if (userToRemove != null)
            {
                _database.EVENT_USERS.Remove(userToRemove);
                _database.SaveChanges();

                return EventActionResult.Success;
            }
            else
            {
                return EventActionResult.UserNotFound;
            }


        }

        /// <summary>
        ///  Update an event's current information
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public EventActionResult UpdateEvent(EventInformation information)
        {
            var updateEvent = _database.EVENTS
                .Where(e => e.ID_PK == information.EventId)
                .FirstOrDefault();

            if (updateEvent != null)
            {
                updateEvent.NAME = information.Name;
                updateEvent.DESCRIPTION = information.Description;

                //reschedule event
                if (updateEvent.TIME_BEGIN.ToUniversalTime() != information.TimeBegin.ToUniversalTime() || updateEvent.TIME_END.ToUniversalTime() != information.TimeEnd.ToUniversalTime())
                {
                    updateEvent.TIME_BEGIN = information.TimeBegin;
                    updateEvent.TIME_END = information.TimeEnd;
                    updateEvent.STATUS = EventStatusType.Rescheduled.ToString();
                }

                updateEvent.TIME_BEGIN = information.TimeBegin;
                updateEvent.TIME_END = information.TimeEnd;
                updateEvent.LOCATION = information.Location;
                updateEvent.ROOM = information.Room;
                updateEvent.TYPE = information.Type.ToString();

                _database.SaveChanges();

                return EventActionResult.Success;
            }
            else
            {
                return EventActionResult.EventNotFound;
            }

        }

        public EventActionResult UpdateUser(EventUser user)
        {
            var existingUser = _database.EVENT_USERS
                .Where(u => u.ID_PK == user.UserId)
                .FirstOrDefault();

            if (existingUser == null) return EventActionResult.UserNotFound;

            existingUser.ROLE = user.Role.ToString();
            _database.SaveChanges();

            return EventActionResult.Success;
        }

        /// <summary>
        ///  Validating is an user has the required permision to do a specific action on a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="authenticatedUser"></param>
        /// <returns></returns>
        public bool IsAuthorized(string userId, string authenticatedUser)
        {
            var userData = _database.EVENT_USERS
                .FirstOrDefault(u => u.ID_PK == userId);

            if(userData == null) return false;

            var authenticatedUserData = _database.EVENT_USERS
                .FirstOrDefault(u => u.EVENT_ID == userData.EVENT_ID && u.STUDENT_NETNAME_FK == authenticatedUser);

            if (authenticatedUserData == null) return false;

            Role userRole;
            Enum.TryParse(userData.ROLE, out userRole);

            Role authenticatedUserRole;
            Enum.TryParse(authenticatedUserData.ROLE, out authenticatedUserRole);


            // Creator, Mod, Scanner, Attendee
            // 0, 1, 2, 3 ranks  lower is better 
            return userRole >= authenticatedUserRole;
            
        }

        /// <summary>
        ///  Validating if an user has the required permision to do a specific action on a event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="authenticatedUser"></param>
        /// <param name="requiredRole"></param>
        /// <param name="exactRole"></param>
        /// <returns></returns>
        public bool IsAuthorized(string eventId, string authenticatedUser, Role requiredRole, bool exactRole)
        {
            var eventUserData = _database.EVENT_USERS
                .FirstOrDefault(e => e.EVENT_ID == eventId && e.STUDENT_NETNAME_FK == authenticatedUser);

            if (eventUserData == null) return false;


            Role currentRole;
            Enum.TryParse(eventUserData.ROLE, out currentRole);

            if (exactRole)
            {
                return currentRole == requiredRole;
            }

            // Creator, Mod, Scanner, Attendee
            // 0, 1, 2, 3 ranks  lower is better 
            return requiredRole >= currentRole;
        }

    }
}
