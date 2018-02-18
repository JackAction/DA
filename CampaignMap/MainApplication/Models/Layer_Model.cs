﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApplication
{
    public class Layer_Model
    {
        public string Name { get; set; }

        public Guid Guid { get; set; }

        public bool IsSelectedForVisibilityHandling { get; set; }

        public bool IsSelectedForNewStroke { get; set; }

        public bool IsSelectedInExistingStroke { get; set; }
    }
}
