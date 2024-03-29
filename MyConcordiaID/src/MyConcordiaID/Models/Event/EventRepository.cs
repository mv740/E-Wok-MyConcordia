﻿using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using MyConcordiaID.Models.Event.Statistic;

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
        public async Task<IEnumerable<EventInformation>> GetActiveEventsAsync()
        {
            var today = DateTime.UtcNow;

            var activeEvents = await _database.EVENTS
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
                .OrderByDescending(e => e.TimeBegin)
                .ToListAsync();

            return activeEvents;
        }

        /// <summary>
        /// Get a list of all events where the user is part of the organization team (creator, mod, scanner) 
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AvailableEvent>> GetAdminEventsAsync(string netname)
        {

            var user = _database.STUDENTS
                .FirstOrDefault(u => u.NETNAME == netname);

            if (user != null)
            {
                var attendee = Role.Attendee.ToString();
                var today = DateTime.UtcNow;

                var studentEvents = await _database.EVENT_USERS
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
                    .ToListAsync();

                //Sort list of dates closest to current date
                var ordederedStudentEvents =  studentEvents
                    .OrderBy(n => (today - n.Information.TimeBegin).Duration())
                    .ThenBy(n => (today - n.Information.TimeEnd).Duration())
                    .ToList();

                return ordederedStudentEvents;
            }
            return null;

        }

        /// <summary>
        ///  Get all the open events that user can participate and all those that he is registered to or is part of the organization team
        /// </summary>
        /// <param name="netname"></param>
        /// <returns>List of events</returns>
        public async Task<IEnumerable<AvailableEvent>> GetAttendeeEventsAsync(string netname)
        {
            var today = DateTime.UtcNow;
            var cancelled = EventStatusType.Cancelled.ToString();
            var attendee = Role.Attendee.ToString();


            // event can't be canceled or expired 
            var events = await _database.EVENT_USERS
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
                .ToListAsync();

            var open = EventType.Open.ToString();


            // event must be open & not canceled or expired
            var openEvents = await _database.EVENTS
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
                .ToListAsync();


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
        public async Task<EventInformation> GetEventByIdAsync(string eventId)
        {

            var currentEvent = await _database.EVENTS
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
                .FirstOrDefaultAsync();

            return currentEvent;
        }

        /// <summary>
        ///  Get a list of all the events
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<EventInformation>> GetEventsAsync()
        {
            var events = await _database.EVENTS
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
                .OrderByDescending(e => e.TimeBegin)
                .ToListAsync();

            return events;
        }

        /// <summary>
        ///  Get a list of all events that have this specifc status 
        /// </summary>
        /// <param name="eventStatus"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EventInformation>> GetEventsByStatusAsync(EventStatusType eventStatus)
        {
            var status = eventStatus.ToString();

            var events = await _database.EVENTS
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
                .OrderByDescending(e => e.TimeBegin)
                .ToListAsync();

            return events;

        }

        /// <summary>
        ///  Find all the users going to a specific event
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="orderUserOnTop"></param>
        /// <param name="netName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EventUserInformation>> GetEventUsersAsync(string eventId, bool orderUserOnTop, string netName)
        {

            var selectedEvent = await _database.EVENTS
                .FirstOrDefaultAsync(e => e.ID_PK == eventId);

            if (selectedEvent != null)
            {
                var eventUsers = await _database.EVENT_USERS
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
                    .ToListAsync();

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
        public async void InsertEvent(NewEvent information, string netname)
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
            await _database.SaveChangesAsync();


            var newOwner = new EVENT_USERS
            {
                STUDENT_NETNAME_FK = netname,
                ROLE = Role.Creator.ToString(),
                STATUS = UserStatus.EventOrganizer.ToString(),
                EVENT_ID = newEvent.ID_PK
            };

            _database.EVENT_USERS.Add(newOwner);
            await _database.SaveChangesAsync();


        }

        /// <summary>
        ///  Add a user to an event if he exist in the system 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<EventActionResult> InsertUserAsync(NewEventUser user)
        {

            var selectedEvent = await _database.EVENTS
                .Where(e => e.ID_PK == user.EventID)
                .FirstOrDefaultAsync();

            if (selectedEvent != null)
            {
                var netName = "";
                if (!string.IsNullOrEmpty(user.UserNetname))
                {
                    var student = await _database.STUDENTS
                        .Where(s => s.NETNAME == user.UserNetname)
                        .FirstOrDefaultAsync();

                    if (student != null)
                    {
                        netName = student.NETNAME;
                    }
                }
                else if (user.UserId != null)
                {
                    var student = await _database.STUDENTS
                        .Where(s => s.ID == user.UserId)
                        .FirstOrDefaultAsync();

                    if (student != null)
                    {
                        netName = student.NETNAME;
                    }

                }

                if (!string.IsNullOrEmpty(netName))
                {
                    //student does exist 


                    var userExist = await _database.EVENT_USERS
                        .Where(u => u.EVENT_ID == user.EventID && u.STUDENT_NETNAME_FK == netName)
                        .FirstOrDefaultAsync();

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
                    await _database.SaveChangesAsync();

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
        public async Task<EventActionResult> RemoveEventAsync(EventCancelled cancellation)
        {

            var removeEvent = await _database.EVENTS
                .FirstOrDefaultAsync(e => e.ID_PK == cancellation.EventId);

            if (removeEvent != null)
            {
                //canceled event
                removeEvent.STATUS = EventStatusType.Cancelled.ToString();
                await _database.SaveChangesAsync();

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
        public async Task<EventActionResult> RemoveUserAsync(EventUser user)
        {
            var userToRemove = await _database.EVENT_USERS
                .FirstOrDefaultAsync(u => u.ID_PK == user.UserId);

            if (userToRemove != null)
            {
                _database.EVENT_USERS.Remove(userToRemove);
                await _database.SaveChangesAsync();

                return EventActionResult.Success;
            }
            return EventActionResult.UserNotFound;
        }

        /// <summary>
        ///  Update an event's current information
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public async Task<EventActionResult> UpdateEventAsync(EventInformation information)
        {
            var updateEvent = await _database.EVENTS
                .FirstOrDefaultAsync(e => e.ID_PK == information.EventId);

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

                await _database.SaveChangesAsync();

                return EventActionResult.Success;
            }
            else
            {
                return EventActionResult.EventNotFound;
            }

        }

        /// <summary>
        ///  Update an user's role 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<EventActionResult> UpdateUserAsync(EventUser user)
        {
            var existingUser = await _database.EVENT_USERS
                .FirstOrDefaultAsync(u => u.ID_PK == user.UserId);

            if (existingUser == null) return EventActionResult.UserNotFound;

            existingUser.ROLE = user.Role.ToString();
            await _database.SaveChangesAsync();

            return EventActionResult.Success;
        }


        /// <summary>
        ///  Retrieve specific data about an event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<EventStatistic> GetEventStatisticAsync(string eventId)
        {
            var myEvent = await _database.EVENTS
                .FirstOrDefaultAsync(e => e.ID_PK == eventId);

            if (myEvent == null) return null;

            //event exist therefore we can look for the users
            var users = await _database.EVENT_USERS
                .Where(user => user.EVENT_ID == eventId)
                .GroupBy(item => item.ROLE)
                .Select(group => new 
                {
                    Role = @group.Key,
                    Items = @group.Select(item => new
                    {
                        Role = item.ROLE,
                        EventId = item.EVENT_ID,
                        Status = item.STATUS,
                        NetName = item.STUDENT_NETNAME_FK
                        
                    })
                } )
                .ToListAsync();

            //return users;


            //Each user type
            var attendees = users
                .FirstOrDefault(s => s.Role == Role.Attendee.ToString());

            var mods = users
                .FirstOrDefault(m => m.Role == Role.Mod.ToString());

            var scanners = users
                .FirstOrDefault(s => s.Role == Role.Scanner.ToString());

            //How many of each attendee status type
            // var registered = attendees.
            var nbOfAttendees =
                attendees?.Items.Where(a => a.Status == UserStatus.Attending.ToString()).ToList().Count ?? 0;

            var nbOfRegistered =
                attendees?.Items.Where(a => a.Status == UserStatus.Registered.ToString()).ToList().Count ?? 0;

            var nbOfTracking =
                attendees?.Items.Where(a => a.Status == UserStatus.Tracking.ToString()).ToList().Count ?? 0;

            var statistic = new EventStatistic()
            {
                Administration = new Administration()
                {
                    Creator = 1,
                    Mods = mods?.Items.Count() ?? 0,
                    Scanners = scanners?.Items.Count() ??0
                },

                Attendees = new Attendees()
                {
                    Attending = nbOfAttendees,
                    Registered = nbOfRegistered,
                    Tracking = nbOfTracking 
                }
            };

            return statistic;
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

            if (userData == null) return false;

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
