using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Dac.Entity;
using System.IO;
using Dac.Data;
using System.Drawing;
using Biz.WebMemo;

/// <summary>
/// WebMemoService의 요약 설명입니다.
/// </summary> 
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class WebMemoService : System.Web.Services.WebService
{
  public WebMemoService()
  {
    //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
    //InitializeComponent(); 
  }

  #region -메모 추가
  /// <summary>
  /// 메모 추가
  /// </summary>
  /// <param name="memo">메모</param>
  /// <returns></returns>
  [WebMethod]
  public string Write(Memo memo)
  {
    bool success = SaveImages(memo.Images);

    if (success == true)
    {
      string originalNames = String.Empty;
      string saveNames = String.Empty;

      for (int i = 0; i < memo.Images.Length; i++)
      {
        if (originalNames == String.Empty)
        {
          originalNames += memo.Images[i].OriginalName;
          saveNames += memo.Images[i].SaveName;
        }
        else
        {
          originalNames += "," + memo.Images[i].OriginalName;
          saveNames += "," + memo.Images[i].SaveName;
        }
      }

      BizMemo bizMemo = new BizMemo();
      int memoIdx = bizMemo.AddMemo(memo.Title, memo.Content, memo.Reg_ID, memo.Color, originalNames, saveNames);

      if (memoIdx == 0)
      {
        for (int i = 0; i < memo.Images.Length; i++)
        {
          File.Delete(Server.MapPath("~/SaveImages/") + memo.Images[i].SaveName);
        }

        return "저장에 실패하였습니다.";
      }
      else
      {
        string newMemo = String.Empty;
        newMemo += "<div class='brick' style='background-color:" + memo.Color + "; position:absolute;'>";
        newMemo += "<input type='hidden' name='memoID' value='" + memoIdx + "'>";
        newMemo += "<div class='click'>";
        newMemo += "<div class='title' style='font-weight:bold'>" + memo.Title + "</div>";
        newMemo += "<div class='contents'>" + HttpUtility.UrlDecode(memo.Content) + "</div>";
        newMemo += "</div></div>";

        return newMemo;
      }

    }
    else
    {
      return "저장에 실패하였습니다.";
    }

  }
  #endregion

  #region -메모 수정
  /// <summary>
  /// 메모 수정
  /// </summary>
  /// <param name="memo">메모</param>
  /// <returns></returns>

  [WebMethod]
  public string Edit(Memo memo)
  {
    string originalNames = String.Empty;
    string saveNames = String.Empty;

    for (int i = 0; i < memo.Images.Length; i++)
    {
      if (originalNames == String.Empty)
      {
        originalNames += memo.Images[i].OriginalName;
        saveNames += memo.Images[i].SaveName;
      }
      else
      {
        originalNames += "," + memo.Images[i].OriginalName;
        saveNames += "," + memo.Images[i].SaveName;
      }
    }

    BizMemo bizMemo = new BizMemo();

    int updated = bizMemo.EditMemo(memo.ID, memo.Title, memo.Content, memo.Color, memo.Reg_ID, originalNames, saveNames);

    bool result = false;

    if (updated > 0)
    {
      result = SaveImages(memo.Images);
    }

    if (result == true)
    {
      return "수정이 완료되었습니다.";
    }
    else
    {
      return "수정 실패하였습니다.";
    }
  }

  #endregion

  #region -메모삭제
  /// <summary>
  /// 메모삭제
  /// </summary>
  /// <param name="MemoID">메모아이디</param>
  /// <returns></returns>
  [WebMethod]
  public string Remove(int MemoID)
  {
    BizMemo bizMemo = new BizMemo();
    List<ImageFile> imageList = bizMemo.RemoveMemo(MemoID);

    if (imageList.Count > 0)
    {
      foreach (ImageFile image in imageList)
      {
        File.Delete(Server.MapPath("~/SaveImages/") + image.SaveName);
      }
    }
    return String.Empty;
  }

  #endregion

  #region -메모리스트 가져오기
  /// <summary>
  /// 메모리스트 가져오기
  /// </summary>
  /// <param name="MemberNo">메모번호</param>
  /// <param name="Type">타입</param>
  /// <param name="Search">검색어</param>
  /// <returns>메모리스트</returns>
  [WebMethod]
  public string MemoList(string MemberNo, string Type, string Search)
  {
    BizMemo bizMemo = new BizMemo();
    List<Memo> memoList = bizMemo.GetMemoListByCondition(MemberNo, Type, Search);

    foreach (Memo memo in memoList)
    {
      Context.Response.Output.Write("<div class='brick' style='background-color:" + memo.Color + "; position:absolute;'>");
      Context.Response.Output.Write("<input type='hidden' name='memoID' value='" + memo.ID + "'>");
      Context.Response.Output.Write("<div class='click'>");
      Context.Response.Output.Write("<div class='title' style='font-weight:bold'>" + memo.Title + "</div>");
      Context.Response.Output.Write("<div class='contents'>" + HttpUtility.UrlDecode(memo.Content) + "</div>");
      Context.Response.Output.Write("</div></div>");
    }
    Context.Response.End();
    return String.Empty;
  }

  #endregion

  #region -메모작성 모달창
  /// <summary>
  /// 메모작성 모달창
  /// </summary>
  /// <returns></returns>
  [WebMethod]
  public string WriteMemoModal()
  {
    Context.Response.Output.Write("<div class='modal-dialog'>");
    Context.Response.Output.Write("<div class='modal-content'>");
    Context.Response.Output.Write("<form>");
    Context.Response.Output.Write("<div class='modal-header'>");
    Context.Response.Output.Write("<button type='button' class='close' data-dismiss='modal' aria-hidden='true' id='btnFindClose'>×</button><br />");
    Context.Response.Output.Write("<h4 class='modal-title' contenteditable='true'><font>Title</font></h4>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<div class='modal-body' contenteditable='true'><font>Content</font></div>");
    Context.Response.Output.Write("<div class='modal-footer'>");
    Context.Response.Output.Write("<div id='colorPicker'>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<input type='file' id='imgFileUpload' onchange='showImagePreview(this)' /><a href='#' id='uploadImage'><img src='/Common/Img/image82.png' /></a>");
    Context.Response.Output.Write("<button id='btnWrite' class='btn btn-default'>완료</button>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</form>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</div>");
    Context.Response.End();
    return String.Empty;
  }

  #endregion

  #region -메모디테일 모달창
  /// <summary>
  /// 메모디테일 모달창
  /// </summary>
  /// <param name="Idx">메모아이디</param>
  [WebMethod]
  public string ViewMemoModal(int Idx)
  {
    BizMemo bizMemo = new BizMemo();
    Memo memo = bizMemo.GetMemoDetailByIdx(Idx);

    Context.Response.Output.Write("<div class='modal-dialog'>");
    Context.Response.Output.Write("<div class='modal-content' style='background-color:" + memo.Color + "'>");
    Context.Response.Output.Write("<form>");
    Context.Response.Output.Write("<div class='modal-header'>");
    Context.Response.Output.Write("<button type='button' class='close' data-dismiss='modal' aria-hidden='true' id='btnFindClose'>×</button><br />");
    Context.Response.Output.Write("<h4 class='modal-title' contenteditable='true'>" + memo.Title + "</h4>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<div class='modal-body' contenteditable='true'>" + HttpUtility.UrlDecode(memo.Content));
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("<div class='modal-footer'>");
    if (memo.Del_Flag.Equals("Y"))
    {
      Context.Response.Output.Write("<button id='btnDelete' class='btn btn-danger'>완전 삭제</button>");
      Context.Response.Output.Write("<button id='btnReturn' class='btn btn-success'>복원</button>");
    }
    else
    {
      Context.Response.Output.Write("<div id='colorPicker'>");
      Context.Response.Output.Write("</div>");
      Context.Response.Output.Write("<input type='file' id='imgFileUpload' onchange='showImagePreview(this)' /><a href='#' id='uploadImage'><img src='/Common/Img/image82.png' /></a>");
      
      if (memo.Important.Equals("Y"))
      {
        Context.Response.Output.Write("<button id='btnImportant' class='btn btn-default'>메인이동</button>");
      }
      else
      {
        Context.Response.Output.Write("<button id='btnImportant' class='btn btn-default'>중요 보관함</button>");
      }
      Context.Response.Output.Write("<button id='btnDelete' class='btn btn-danger'>삭제</button>");
      Context.Response.Output.Write("<button id='btnEdit' class='btn btn-default'>완료</button>");
    }
    
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</form>");
    Context.Response.Output.Write("</div>");
    Context.Response.Output.Write("</div>");
    Context.Response.End();
    return String.Empty;

  }

  #endregion

  #region -메모 중요보관함으로 이동
  /// <summary>
  /// 메모 중요보관함으로 이동
  /// </summary>
  /// <param name="MemoID">메모아이디</param>
  /// <returns></returns>
  [WebMethod]
  public string SetImportant(int MemoID)
  {
    BizMemo bizMemo = new BizMemo();
    bizMemo.SetImportantMemo(MemoID);
    return String.Empty;
  }

  #endregion

  #region -메모 원위치로 복원
  /// <summary>
  /// 메모 원위치로 복원
  /// </summary>
  /// <param name="MemoID">메모 아이디</param>
  /// <returns></returns>
  [WebMethod]
  public string RetrunMemo(int MemoID)
  {
    BizMemo bizMemo = new BizMemo();
    bizMemo.ReturnMemo(MemoID);
    return String.Empty;
  }

  #endregion

  #region -이미지 저장
  /// <summary>
  /// 이미지 저장
  /// </summary>
  /// <param name="images">이미지</param>
  /// <returns></returns>
  private bool SaveImages(ImageFile[] images)
  {
    try
    {
      foreach (ImageFile image in images)
      {
        if (!image.Src.Substring(0, 11).Equals("/SaveImages"))
        {
          var imageData = image.Src;
          string imageDataParsed = imageData.Substring(imageData.IndexOf(',') + 1);
          byte[] imageBytes = Convert.FromBase64String(imageDataParsed);
          Stream imageStream = new MemoryStream(imageBytes);

          Image img = Image.FromStream(imageStream);
          img.Save(Server.MapPath("~/SaveImages/" + image.SaveName));
        }
      }
    }
    catch (Exception)
    {
      return false;
    }

    return true;
  }

  #endregion

  #region -메모 순번 저장
  /// <summary>
  /// 메모 순번 저장
  /// </summary>
  /// <param name="MemoIDList">메모 아이디 리스트</param>
  /// <param name="OrderList">메모 순번 리스트</param>
  /// <returns></returns>
  [WebMethod]
  public string OrderMemo(string MemoIDList, string OrderList)
  {
    BizMemo bizMemo = new BizMemo();
    bizMemo.SetOrder(MemoIDList, OrderList);
    return String.Empty;

  }
  #endregion

}