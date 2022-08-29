using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{

    public class ScreenCheModel
    {
        public string CheNO { get; set; }
        public int JizuIndex { get; set; }
        public int JizuId { get; set; }
    }

    //表格信息
    public class GridData
    {
        public string MaterialName = "";    //材料名称
        public string Supplier = "";        //供应商
        public string CheckState = "";      //检查状态
        public string Standard = "";        //规格
        public string Field = "";           //产地--停用
        public string PotNo = "";           //罐号
        public string Amount = "";           //库存量
    }

    //表格样式
    public class GridStyle
    {
        public GridStyle()
        {
            fontSize = 10;
            leftMargin = 2;
            topMargin = 12;
            MaxCharNum = 8;
        }

        //字体大小
        public float fontSize { get; set; }

        //字体左边偏移
        public float leftMargin { get; set; }

        //字体上方偏移
        public float topMargin { get; set; }

        //表格高度,默认为显示屏高度
        public int GridHeight { get; set; }

        //表格宽度,默认为显示屏宽度
        public int GridWidth { get; set; }

        //表格最多显示多少个字数 
        public int MaxCharNum { get; set; }
    }

}
