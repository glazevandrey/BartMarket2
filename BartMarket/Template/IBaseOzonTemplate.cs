﻿using System.Collections.Generic;

namespace BartMarket.Template
{
    public interface IBaseOzonTemplate
    {
        public string Name { get; set; }
        public string PathToTemplate { get; set; }
        public List<string> KeyWords { get; set; }

        public string Parse(int count);

    }
}
