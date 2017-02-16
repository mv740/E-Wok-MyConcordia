﻿using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Event;
using MyConcordiaID.Models.Student;
using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Claims;
using System.Linq;
using System.Threading.Tasks;

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
                .Where(u => u.NETNAME == netname)
                .FirstOrDefault();

            if (user != null)
            {
                var attendee = Role.Attendee.ToString();

                var studentEvents = _database.EVENT_USERS
                    .Where(e => e.STUDENT_NETNAME_FK == netname && e.ROLE != attendee)
                    .Select(e => new
                    {
                        e.ROLE,
                        e.STATUS,
                        e.EVENT_ID,
                        e.EVENT
                    });

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
            //order events
            events = events
               .Concat(openEvents)
               .GroupBy(x => x.Information.EventId)
               .Select(s => s.First())
               .OrderByDescending(e => e.Information.TimeBegin)
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
        /// <returns></returns>
        public IEnumerable<EventUserInformation> GetEventUsers(string eventId)
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
                            ID = u.STUDENT.ID,
                            NetName = u.STUDENT.NETNAME,
                            FirstName = u.STUDENT.FIRSTNAME,
                            LastName = u.STUDENT.LASTNAME
                        }
                    });

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

            EVENT newEvent = new EVENT
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


            EVENT_USERS newOwner = new EVENT_USERS
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

                    var status = UserStatus.Tracking.ToString(); ;
                    if (string.Equals(selectedEvent.TYPE, EventType.Closed.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        status = UserStatus.Registered.ToString();
                    }

                    if (user.Role != Role.Attendee)
                    {
                        status = UserStatus.EventOrganizer.ToString();
                    }


                    EVENT_USERS newUser = new EVENT_USERS
                    {
                        STUDENT_NETNAME_FK = user.UserNetname,
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

            ScannerResult processResult = new ScannerResult
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
                    EVENT_USERS newUser = new EVENT_USERS
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

            if (existingUser != null)
            {
                existingUser.ROLE = user.Role.ToString();
                _database.SaveChanges();

                return EventActionResult.Success;
            }

            return EventActionResult.UserNotFound;
        }
    }
}