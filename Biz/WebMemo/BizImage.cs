using Dac.Entity;
using Dac.WebMemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.WebMemo
{
  public class BizImage
  {
    #region -이미지리스트 가져오기
    /// <summary>
    /// 이미지리스트 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ImageFile> GetImageNameList()
    {
      DacImage dacImage = new DacImage();
      return dacImage.SelectImageList();
    }
    #endregion
  }
}