using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Domain;
using TCZNGB.Core.Domain.Response;

namespace TCZNGB.Service.ZNGB_ControlRoadgate
{
    public interface IControlRoadgateService
    {
        ResponseContext ControlRoadgate(ControlRoadgateModel model);
    }
}
