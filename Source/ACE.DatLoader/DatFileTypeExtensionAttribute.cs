﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DatFileTypeExtensionAttribute : Attribute
    {
        public string FileExtension { get; set; }

        public DatFileTypeExtensionAttribute(string fileExtension)
        {
            this.FileExtension = fileExtension;
        }
    }
}
