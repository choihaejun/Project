using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Biz.WebMemo;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

public partial class Public_FindInfo : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (Request.Form["type"] == "I")
    {
      FindID();
    }
    else
    {
      CreateNewPassword();
    }
  }

  #region -아이디 찾기
  /// <summary>
  /// 아이디찾기
  /// </summary>
  protected void FindID()
  {
    BizMember bizMember = new BizMember();

    string email = bizMember.FindID(Request.Form["name"], Request.Form["phone"]);

    Response.Write(email);
  }

  #endregion

  #region -새비밀번호 생성후 이메일 발송
  /// <summary>
  /// 새비밀번호 생성후 이메일 발송
  /// </summary>
  protected void CreateNewPassword()
  {
    BizMember bizMember = new BizMember();

    string newPW = Path.GetRandomFileName().Replace(".", "");

    int updated = bizMember.FindPassword(Request.Form["email"],                         /* 이메일 */
                                         Request.Form["name"],                          /* 이름 */
                                         newPW                                          /* 비밀번호 */
                                         );
    if (updated > 0)
    {
      try
      {
        MailMessage mailMsg = new MailMessage();

        mailMsg.To.Add(new MailAddress(Request.Form["email"]));

        mailMsg.From = new MailAddress("Admin@pingoli.com", "핑고리");

        mailMsg.Subject = "임시 비밀번호 발송";
        string text = Request.Form["name"] + " 님의 새로운 비밀번호는 " + newPW + " 입니다.";
        mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));

        SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_5d3fe7bc8e8fec10790f72c6760bc104@azure.com", "YkrJBg0X5ePy9mu");
        smtpClient.Credentials = credentials;

        smtpClient.Send(mailMsg);
      }
      catch (Exception e)
      {
        Response.Write(e.Message);

      }
      finally
      {
        Response.Write("이메일로 임시비밀번호가 발송되었습니다.");
      }
    }
    else
    {
      Response.Write("잘못된 정보입니다. 다시 입력해주세요.");
    }
  }

  #endregion

}