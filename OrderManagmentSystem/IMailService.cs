﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces.Services
{
    public interface IMailService
    {
        public void SendEmail(Email email);
    }
}
