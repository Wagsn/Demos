using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    public class LedScreenSettingModel
    {
        protected int _screenID;

        protected string _screenName;
        protected int _screenIndex;
        protected int _screenTypeID;
        protected bool _isStop;

        protected int _fixedHeight;
        protected string _fixedFont;
        protected int _fixedFontSize;
        protected bool _fixedBold;
        protected int _fixedLeftMargin;

        protected int _titleHeight;
        protected int _titleFontSize;

        protected int _behindWaitingSpace;

        protected string _rollingInfoFont;
        protected int _rollingInfoFontSize;
        protected bool _rollingInfoBold;
        protected int _rollingInfoHeight;
        protected int _rollingInfoType;
        protected int _rollingInfoSpeed;

        protected int _gridFontSize;
        protected int _gridLeftMargin;
        protected int _gridTopMargin;
        protected int _girdTextLength;


        public LedScreenSettingModel()
        {
            _fixedHeight = 16;
            _fixedFont = "宋体";
            _fixedFontSize = 10;
            _fixedBold = false;
            _fixedLeftMargin = 32;
            _titleHeight = 16;
            _titleFontSize = 10;
            _behindWaitingSpace = 0;
            _rollingInfoFont = "宋体";
            _rollingInfoFontSize = 10;
            _rollingInfoBold = true;
            _rollingInfoHeight = 16;
            _rollingInfoType = 2;
            _rollingInfoSpeed = 9;
            _gridFontSize = 8;
            _gridLeftMargin = 2;
            _gridTopMargin = 12;
            _girdTextLength = 10;
        }


        public virtual int ScreenID
        {
            get { return _screenID; }
            set { _screenID = value; }
        }

        // "屏幕名称"
        public virtual string ScreenName
        {
            get { return _screenName; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for ScreenName", value, value.ToString());
                _screenName = value;
            }
        }

        // "屏幕序号"
        public virtual int ScreenIndex
        {
            get { return _screenIndex; }
            set { _screenIndex = value; }
        }

        // "屏幕类型ID"
        public virtual int ScreenTypeID
        {
            get { return _screenTypeID; }
            set { _screenTypeID = value; }
        }

        // "是否停用"
        public virtual bool IsStop
        {
            get { return _isStop; }
            set { _isStop = value; }
        }

        // "固定区行高"
        public virtual int FixedHeight
        {
            get { return _fixedHeight; }
            set { _fixedHeight = value; }
        }

        // "固定区字体"
        public virtual string FixedFont
        {
            get { return _fixedFont; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for FixedFont", value, value.ToString());
                _fixedFont = value;
            }
        }

        // "固定区字号"
        public virtual int FixedFontSize
        {
            get { return _fixedFontSize; }
            set { _fixedFontSize = value; }
        }

        // "固定区是否粗体"
        public virtual bool FixedBold
        {
            get { return _fixedBold; }
            set { _fixedBold = value; }
        }

        // "固定区左边距"
        public virtual int FixedLeftMargin
        {
            get { return _fixedLeftMargin; }
            set { _fixedLeftMargin = value; }
        }

        // "机组标题行高"
        public virtual int TitleHeight
        {
            get { return _titleHeight; }
            set { _titleHeight = value; }
        }

        // "机组标题字号"
        public virtual int TitleFontSize
        {
            get { return _titleFontSize; }
            set { _titleFontSize = value; }
        }

        // "等待后面空白"
        public virtual int BehindWaitingSpace
        {
            get { return _behindWaitingSpace; }
            set { _behindWaitingSpace = value; }
        }

        // "滚动信息字体"
        public virtual string RollingInfoFont
        {
            get { return _rollingInfoFont; }
            set
            {
                if (value != null && value.Length > 50)
                    throw new ArgumentOutOfRangeException("Invalid value for RollingInfoFont", value, value.ToString());
                _rollingInfoFont = value;
            }
        }

        // "滚动信息字号"
        public virtual int RollingInfoFontSize
        {
            get { return _rollingInfoFontSize; }
            set { _rollingInfoFontSize = value; }
        }

        // "滚动信息粗体"
        public virtual bool RollingInfoBold
        {
            get { return _rollingInfoBold; }
            set { _rollingInfoBold = value; }
        }

        // "滚动信息行高"
        public virtual int RollingInfoHeight
        {
            get { return _rollingInfoHeight; }
            set { _rollingInfoHeight = value; }
        }

        // "滚动信息显示方式"
        public virtual int RollingInfoType
        {
            get { return _rollingInfoType; }
            set { _rollingInfoType = value; }
        }

        // "滚动信息速度"
        public virtual int RollingInfoSpeed
        {
            get { return _rollingInfoSpeed; }
            set { _rollingInfoSpeed = value; }
        }

        // "表格字体大小"
        public virtual int GridFontSize
        {
            get { return _gridFontSize; }
            set { _gridFontSize = value; }
        }

        // "表格字体左边偏移"
        public virtual int GridLeftMargin
        {
            get { return _gridLeftMargin; }
            set { _gridLeftMargin = value; }
        }

        // "表格字体上方偏移"
        public virtual int GridTopMargin
        {
            get { return _gridTopMargin; }
            set { _gridTopMargin = value; }
        }

        // "表格字数上限"
        public virtual int GirdTextLength
        {
            get { return _girdTextLength; }
            set { _girdTextLength = value; }
        }
    }
}
