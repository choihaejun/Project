using Biz.WebMemo;
using Dac.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Admin : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      if(Request.Cookies["LoginInfo"] == null || Request.Cookies["LoginInfo"]["LVL"].ToString() == "0")
      {
        Response.Redirect("/Public/Login.aspx");
      }

      GetMemberList();
    }
  }

  #region -회원리스트 가져오기
  /// <summary>
  /// 회원리스트 가져오기
  /// </summary>
  protected void GetMemberList()
  {
    BizMember bizMember = new BizMember();
    rptMemberList.DataSource = bizMember.GetMemberList();
    rptMemberList.DataBind();
  }
  #endregion

  #region -회원삭제
  /// <summary>
  /// 회원삭제
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void lbtnDeleteMember_Click(object sender, EventArgs e)
  {
    LinkButton lbtnDelete = (LinkButton)sender;
    HiddenField hfMemberNo = (HiddenField)lbtnDelete.Parent.FindControl("hfMemberNo");

    BizMember bizMember = new BizMember();
    int result = bizMember.RemoveMember(hfMemberNo.Value);

    if (result > 0)
    {
      GetMemberList();
    }
  }

  #endregion

  #region -이미지 정리
  /// <summary>
  /// 이미지 정리
  /// </summary>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  protected void btnImageDelete_ServerClick(object sender, EventArgs e)
  {
    BizImage bizImage = new BizImage();
    List<ImageFile> imageList = bizImage.GetImageNameList();

    string[] imageFile = Directory.GetFiles(Server.MapPath(@"/SaveImages/"));

    for(int i = 0; i < imageFile.Length; i++)
    {
      bool flag = false;

      string[] imgPath = imageFile[i].Split('\\');

      foreach (ImageFile image in imageList)
      {
        if (imgPath[imgPath.Length - 1].Equals(image.SaveName))
        {
          flag = true;
          break;
        }
      }
  
      if(flag == false)
      {
        File.Delete(Server.MapPath(@"/SaveImages/") + imgPath[imgPath.Length - 1]);
      }
    }

    ScriptManager.RegisterStartupScript(this,
                                        Type.GetType("System.String"),
                                        "",
                                        "javascript:window.alert('이미지정리가 완료되었습니다!');",
                                        true
                                        );

  }

  #endregion
}