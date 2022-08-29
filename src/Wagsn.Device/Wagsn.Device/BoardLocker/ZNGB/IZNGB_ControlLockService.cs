using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Domain;
using TCZNGB.Core.Domain.Response;

namespace TCZNGB.Service.ZNGB_ControlLock
{
    public interface IZNGB_ControlLockService
    {
        ResponseContext ControlLock(ControlLockModel model);
    }
}
