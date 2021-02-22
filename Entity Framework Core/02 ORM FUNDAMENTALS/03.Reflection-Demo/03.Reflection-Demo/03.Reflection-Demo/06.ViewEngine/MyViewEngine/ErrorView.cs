﻿namespace ViewEngineDemo.MyViewEngine
{
    public class ErrorView : IView
    {
        private readonly string errors;

        public ErrorView(string errors)
        {
            this.errors = errors;
        }

        public string GetHtml(object model)
        {
            return this.errors;
        }
    }
}
