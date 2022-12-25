using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TravelApi.Calendar;
using static Google.Apis.Calendar.v3.Data.Event;

namespace TravelApi.Helpers
{
    public class GoogleCalendarHelper
    {
        protected GoogleCalendarHelper()
        {

        }
        public static async Task<Event> CreateGoogleCalendar(GoogleCalendar request)
        {
            //var arrList = request.EmailNhan.Split(",");
            var EventAttendee = new List<EventAttendee>();
            //foreach (var item in arrList)
            //{
                var eventAttendee = new EventAttendee() { Email = request.EmailNhan, Organizer = true, DisplayName = "VUA DU LỊCH", Comment = "Chuyến đi mang lại cảm giác tươi mới" };
                EventAttendee.Add(eventAttendee);
            //}
            string[] Scopes = {"https://www.googleapis.com/auth/calendar"};
            string ApplicationName = "Google Calendar API";
            UserCredential credential;
            using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(),"Cre","cre.json"),FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath,true)).Result;
            }
            // define service
            var services = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
            // define request

            Event eventCalendar = new Event() {
                Recurrence = new String[] { "RRULE:FREQ=WEEKLY;BYDAY=MO" },
                ColorId = "1",
                Organizer = new OrganizerData 
                { 
                    Email = "ng.hglong102@gmail.com",
                    DisplayName = "Vua" ,
                    Self = true
                },
                Attendees = EventAttendee,
                Creator = new CreatorData { DisplayName ="Vua",
                Email = "longnghg100220@gmail.com"
                },
                GuestsCanSeeOtherGuests = false,
                GuestsCanModify = false,
                Summary = request.Summary,
                Location = request.Location,
                Start = new EventDateTime
                {
                    DateTime = request.Start,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                End = new EventDateTime
                {
                    DateTime = request.End,
                    TimeZone = "Asia/Ho_Chi_Minh"
                },
                Description = request.Description
                
            };
           
            var eventRequest = services.Events.Insert(eventCalendar, "primary");
            eventRequest.SendNotifications = true;
         
            var requestCreate =await eventRequest.ExecuteAsync();
            return requestCreate;

        }
    }
}
// https://referencesource.microsoft.com/#System.Core/System/Linq/Enumerable.cs,d35db0dea6ae310a