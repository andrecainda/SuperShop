using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SuperShop.Helpers
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewNamme)
        {
            ViewName = viewNamme;
            StatusCode = (int)HttpStatusCode.NotFound;   
        }
    }
}
