﻿namespace Project_Invoice_MAUI.SaveFileHelper
{
    public interface ISave
    {
        //Method to save document as a file and view the saved document
        Task SaveAndView(string filename, string contentType, MemoryStream stream);
    }
}
