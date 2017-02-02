using OracleEntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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


        public dynamic GetEvent(string eventId)
        {
            var currentEvent = _database.EVENTS
                .Where(e => e.ID_PK == eventId)
                .Select(e => new
                {
                    e.ID_PK,
                    e.NAME,
                    e.DESCRIPTION,
                    e.LOCATION,
                    e.ROOM,
                    e.TIME_BEGIN,
                    e.TIME_END,
                    e.TYPE
                })
                .FirstOrDefault();

            return currentEvent;
        }

        public IEnumerable<dynamic> GetEvents()
        {
            var events = _database.EVENTS
                .Select(e => new
                {
                    e.ID_PK,
                    e.NAME,
                    e.DESCRIPTION,
                    e.LOCATION,
                    e.ROOM,
                    e.TIME_BEGIN,
                    e.TIME_END,
                    e.TYPE
                })
                .OrderByDescending(e => e.TIME_BEGIN);

            return events;
        }

        public void InsertEvent(EventInformation information, string netname)
        {

            EVENT newEvent = new EVENT
            {
                NAME = information.Name,
                DESCRIPTION = information.Description,
                TIME_BEGIN = DateTime.Parse(information.TimeBegin),
                TIME_END = DateTime.Parse(information.TimeEnd),
                LOCATION = information.Location,
                ROOM = information.Room,
                TYPE = information.Type.ToString()

            };

            _database.EVENTS.Add(newEvent);
            _database.SaveChanges();


            EVENT_USERS newOwner = new EVENT_USERS
            {
                STUDENT_NETNAME_FK = netname,
                ROLE = Role.Creator.ToString(),
                STATUS = Status.EventOrganizer.ToString(),
                EVENT_ID = newEvent.ID_PK
            };

            _database.EVENT_USERS.Add(newOwner);
            _database.SaveChanges();

        }

        /// <summary>
        ///  Add a user to an event if he exist in the system 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ActionResult InsertUser(EventUser user)
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

            if(!string.IsNullOrEmpty(netName))
            {
                //student does exist 

                var status = Status.Tracking.ToString(); ;
                if (user.EventType == TYPE.CLOSED)
                {
                    status = Status.Registered.ToString();
                }

                if(user.Role != Role.Attendee)
                {
                    status = Status.EventOrganizer.ToString();
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

                return ActionResult.Success;
            }
            else
            {
                return ActionResult.UserNotFound;
            }


           
        }

        public void RemoveEvent(EventCancellation cancellation)
        {
     
        }

        public void RemoveUser(EventUser user)
        {
            throw new NotImplementedException();
        }

        public void UpdateEvent(EventInformation information)
        {
            throw new NotImplementedException();
        }
    }
}
