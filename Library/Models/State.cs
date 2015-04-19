using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Configurations;
using System.Web.SessionState;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Library.Models
{
    public class State
    {
        public static void StartSession()
        {
            SessionStart = DateTime.Now;
        }

        public static DateTime SessionStart 
        {
            get
            {
                object start = HttpContext.Current.Session[SessionStartKey];
                return (start != null && start is DateTime) ? (DateTime)start : DateTime.MinValue;
            }

            private set
            {
                HttpContext.Current.Session[SessionStartKey] = value;
            }
        }

        public static DateTime SessionEnd 
        {
            get
            {
                return SessionStart.AddMinutes(HttpContext.Current.Session.Timeout);
            }
        }


        public int MinutesLeft
        {
            get
            {
                return (int)Math.Round(SessionEnd.Subtract(DateTime.Now).TotalMinutes, 0);
            }
        }

        [DisplayName("Datenlagerung")]
        public String DataStorage
        {
            get
            {
                return Configuration.DataStorage == null ? "N/A" : Configuration.DataStorage;
            }
        }

        [DisplayName("Datenquelem")]
        public String DataSource
        {
            get
            {
                return Configuration.DataInitSource == null ? "N/A" : Configuration.DataInitSource;
            }
        }

        [DisplayName("max. Bücher per Person")]
        public String MaxBookPerPerson
        {
            get
            {
                return Configuration.MaxBookPerPerson == int.MaxValue ? "N/A" : Configuration.MaxBookPerPerson.ToString();
            }
        }

        public String DataExpiration
        {
            get
            {
                return Configuration.usingSessionCashe ? MinutesLeft.ToString() : "N/A";
            }
        }



        private static string SessionStartKey = "SessionStart";
    }


    
}