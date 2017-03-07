using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyConcordiaID.Helper;
using MyConcordiaID.Models.Event;
using MyConcordiaID.Models.Log;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyConcordiaID.Models.Event.Statistic;

namespace MyConcordiaID.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    public class EventController : Controller
    {

        private ILogRepository _logRepo { get; set; }
        private IEventRepository _eventRepo { get; set; }


        public EventController(ILogRepository logs, IEventRepository events)
        {
            _logRepo = logs;
            _eventRepo = events;
        }

        /// <summary>
        /// Retrieve all the events
        /// </summary>
        /// <returns></returns>
        /// <response code="200">List of events</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetAll()
        {
            var events = _eventRepo.GetEvents();

            return new ObjectResult(events);
        }

        /// <summary>
        ///  Retrieve a specific event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return event information</response>
        /// <response code="404">id is invalid, event doesn't exist</response>
        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetEvent")]
        [ProducesResponseType(typeof(EventInformation), 200)]
        public IActionResult GetById(string id)
        {
            var myEvent = _eventRepo.GetEventById(id);

            if (myEvent == null)
            {
                return NotFound();
            }

            return new ObjectResult(myEvent);
        }

        /// <summary>
        ///  Retrieve all canceled events       
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return canceled events</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("status/canceled")]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetCanceledEvents()
        {
            var canceledEvent = _eventRepo.GetEventsByStatus(EventStatusType.Cancelled);

            return new ObjectResult(canceledEvent);
        }

        /// <summary>
        ///  Retrieve all Postponed events        
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return Postponed events</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("status/postponed")]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetPostponedEvents()
        {
            var canceledEvent = _eventRepo.GetEventsByStatus(EventStatusType.Postponed);

            return new ObjectResult(canceledEvent);
        }
        /// <summary>
        ///  Retrieve all Rescheduled events        
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return Rescheduled events</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("status/rescheduled")]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetResheduleddEvents()
        {
            var canceledEvent = _eventRepo.GetEventsByStatus(EventStatusType.Rescheduled);

            return new ObjectResult(canceledEvent);
        }

        /// <summary>
        ///  Retrieve all scheduled events       
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return scheduled events</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("status/scheduled")]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetScheduledEvents()
        {
            var canceledEvent = _eventRepo.GetEventsByStatus(EventStatusType.Scheduled);

            return new ObjectResult(canceledEvent);
        }

        /// <summary>
        ///  Retrieve all active events, those that are still valid /not expired        
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Return scheduled events</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("status/active")]
        [ProducesResponseType(typeof(IEnumerable<EventInformation>), 200)]
        public IActionResult GetActiveEvents()
        {
            var activeEvents = _eventRepo.GetActiveEvents();

            return new ObjectResult(activeEvents);
        }

        /// <summary>
        ///  Add a user to a event
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">user Submited</response>
        /// <response code="404">user invalid or event not found</response>
        [AllowAnonymous]
        [HttpPost]
        [Route("user")]
        public IActionResult PostEventUser([FromBody] NewEventUser user)
        {
            var result = _eventRepo.InsertUser(user);
            switch (result)
            {
                case EventActionResult.EventNotFound:
                case EventActionResult.UserNotFound:
                    return NotFound();
                case EventActionResult.DuplicateUser:
                    var returnAction = CreatedAtAction("PostEventUser", user);
                    returnAction.StatusCode = StatusCodes.Status409Conflict;
                    return returnAction;

            }

            return Ok();
        }

        /// <summary>
        ///  update a user's role
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">user update</response>
        /// <response code="404">user invalid</response>
        [AllowAnonymous]
        [HttpPut]
        [Route("user")]
        public IActionResult UpdateEventUser([FromBody] EventUser user)
        {
            var result = _eventRepo.UpdateUser(user);
            if (result != EventActionResult.Success)
            {
                return NotFound();
            }
            return Ok();
        }

        /// <summary>
        ///  remove a user from an event
        /// </summary>
        /// <param name="user"></param>
        /// <response code="200">user removed</response>
        /// <response code="404">user invalid</response>
        [AllowAnonymous]
        [HttpDelete]
        [Route("user")]
        public IActionResult DeleteEventUser([FromBody] EventUser user)
        {
            var result = _eventRepo.RemoveUser(user);
            if (result != EventActionResult.Success)
            {
                return NotFound();
            }
            return Ok();
        }


        /// <summary>
        ///  Retrieve a specific event's users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return event users</response>
        /// <response code="404">Event not found</response>
        [ProducesResponseType(typeof(IEnumerable<EventUserInformation>), 200)]
        [AllowAnonymous]
        [HttpGet("{id}/users/{order:bool?}")]
        public IActionResult GetEventUser(string id, [FromRoute] bool order = false)
        {
            //var authenticatedUser = GetAuthenticatedUserNetname();

            var users = _eventRepo.GetEventUsers(id, order, "m_woznia");
            if (users == null)
            {
                //event not found
                return NotFound();
            }

            return new ObjectResult(users);
        }


        /// <summary>
        ///  Retrieve a specific event's statistic
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Return event statistic</response>
        /// <response code="404">Event not found</response>
        [ProducesResponseType(typeof(EventStatistic), 200)]
        [AllowAnonymous]
        [HttpGet("{id}/stats")]
        public IActionResult GetEventStatistic(string id)
        {
            //var authenticatedUser = GetAuthenticatedUserNetname();

            var users = _eventRepo.GetEventStatistic(id).Result;
            if (users == null)
            {
                //event not found
                return NotFound();
            }

            return new ObjectResult(users);
        }


        /// <summary>
        ///  Create a event
        /// </summary>
        /// <param name="newEvent"></param>
        /// <response code="200">event Submited</response>
        [AllowAnonymous]
        [HttpPost]
        public IActionResult PostEvent([FromBody] NewEvent newEvent)
        {

            //need to oauth to get the creator's netname

            _eventRepo.InsertEvent(newEvent, "m_woznia"); //default user for now until everything is tested

            return Ok();
        }


        /// <summary>
        ///  Update a event
        /// </summary>
        /// <param name="information"></param>
        /// <response code="200">event updated</response>
        /// <response code="404">event not found</response>
        [AllowAnonymous]
        [HttpPut]
        public IActionResult UpdateEvent([FromBody] EventInformation information)
        {
            var selectedEvent = _eventRepo.UpdateEvent(information); //default user for now until everything is tested
            if (selectedEvent != EventActionResult.Success)
            {
                return NotFound();
            }
            return Ok();
        }


        /// <summary>
        ///  Cancel a event
        /// </summary>
        /// <param name="cancelEvent"></param>
        /// <response code="200">event canceled</response>
        /// <response code="404">event not found</response>
        [AllowAnonymous]
        [HttpDelete]
        public IActionResult CancelEvent([FromBody] EventCancelled cancelEvent)
        {
            var result = _eventRepo.RemoveEvent(cancelEvent);
            if (result != EventActionResult.Success)
            {
                return NotFound();
            }

            return Ok();
        }

        /// <summary>
        ///  Get a list of events where the user is an administrator of the event
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        /// <response code="200">List of event</response>
        /// <response code="404">user not found</response>
        [AllowAnonymous]
        [HttpGet]
        [Route("admin/{netname}")]
        [ProducesResponseType(typeof(IEnumerable<AvailableEvent>), 200)]
        public IActionResult GetMyAdminEvents(string netname)
        {
            var events = _eventRepo.GetAdminEvents(netname);
            if (events == null)
            {
                //user not found
                return NotFound();
            }
            return new ObjectResult(events);
        }

        /// <summary>
        /// FOR MOBILE
        ///  Get a list of events where the user can participate or scan users
        /// </summary>
        /// <param name="netname"></param>
        /// <returns></returns>
        /// <remarks>Must be authenticated!</remarks>
        /// <response code="200">List of events</response>
        /// <response code="401">Unauthorized</response>
        [Authorize]
        [HttpGet]
        [Route("user")]
        [ProducesResponseType(typeof(IEnumerable<AvailableEvent>), 200)]
        public IActionResult GetAttendeeEvents()
        {
            var authenticatedUser = GetAuthenticatedUserNetname();
            var events = _eventRepo.GetAttendeeEvents(authenticatedUser);

            return new ObjectResult(events);
        }

        /// <summary>
        /// FOR MOBILE
        ///  Scanned user is registed to a event
        /// </summary>
        /// <remarks>Must be authenticated!</remarks>
        /// <returns></returns>
        /// <response code="200">User was found </response>
        /// <response code="404">User id not found </response>
        [Authorize]
        [HttpPost]
        [Route("scanner")]
        [ProducesResponseType(typeof(ScannerResult), 200)]
        public IActionResult PostRegisterScannedUser([FromBody] ScannerUser user)
        {
            var scannerResult = _eventRepo.RegisterScannedUser(user);

            if (scannerResult.Status == ScannerStatus.IdNotFound.ToString())
            {
                return NotFound();
            }

            return new ObjectResult(scannerResult);
        }

        private string GetAuthenticatedUserNetname()
        {
            var firstName = User.FindFirstValue(ClaimTypes.GivenName);
            var lastName = User.FindFirstValue(ClaimTypes.Surname);
            return StudentHelper.GenerateNetName(firstName, lastName);
        }

    }
}
