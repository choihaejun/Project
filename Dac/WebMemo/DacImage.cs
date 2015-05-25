using Dac.Entity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dac.WebMemo
{
  public class DacImage
  {
    #region -변수
    
    SqlConnection conn = null;
    SqlTransaction trx = null;
    string conStr = WebConfigurationManager.ConnectionStrings["PingoliConStr"].ConnectionString;
    
    #endregion
    
    #region -이미지리스트 가져오기
    /// <summary>
    /// 이미지리스트 가져오기
    /// </summary>
    /// <returns></returns>
    public List<ImageFile> SelectImageList()
    {
      conn = new SqlConnection(conStr);

      List<ImageFile> imageList = new List<ImageFile>();

      SqlDataReader rd = SqlHelper.ExecuteReader(conn,"image_list");
      
      while (rd.Read())
      {
        ImageFile image = new ImageFile();
        image.SaveName = rd["SAVE_NAME"].ToString();
        imageList.Add(image);
      }

      if (!rd.IsClosed)
      {
        rd.Close();
      }

      return imageList;
    }

    #endregion
  }
}