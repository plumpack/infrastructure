using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PlumPack.Web
{
    public class BaseController : Controller
    {
        public void AddSuccessMessage(string message, bool persist = false)
        {
            if (persist)
            {
                if (!(TempData["currentSuccessMessages"] is List<string> currentMessages))
                {
                    currentMessages = new List<string>();
                }

                currentMessages.Add(message);
                TempData["currentSuccessMessages"] = currentMessages;
            }
            else
            {
                if (!(ViewData["currentSuccessMessages"] is List<string> currentMessages))
                {
                    currentMessages = new List<string>();
                }

                currentMessages.Add(message);
                ViewData["currentSuccessMessages"] = currentMessages;
            }
        }

        public void AddFailureMessage(string message, bool persist = false)
        {
            if (persist)
            {
                if (!(TempData["currentFailureMessages"] is List<string> currentMessages))
                {
                    currentMessages = new List<string>();
                }
                currentMessages.Add(message);
                TempData["currentFailureMessages"] = currentMessages;
            }
            else
            {
                if (!(ViewData["currentFailureMessages"] is List<string> currentMessages))
                {
                    currentMessages = new List<string>();
                }
                currentMessages.Add(message);
                ViewData["currentFailureMessages"] = currentMessages;
            }
        }
    }
}