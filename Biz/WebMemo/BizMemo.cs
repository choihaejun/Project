using Dac.Entity;
using Dac.WebMemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.WebMemo
{
  public class BizMemo
  {
    #region -메모 저장
    /// <summary>
    /// 메모 저장
    /// </summary>
    /// <param name="Title">제목</param>
    /// <param name="Contents">내용</param>
    /// <param name="MemberNo">멤버번호</param>
    /// <param name="Color">메모지색</param>
    /// <param name="OriginalNames">실제 이미지명</param>
    /// <param name="SaveNames">저장될 이미지명</param>
    /// <returns></returns>
    public int AddMemo(string Title, string Contents, int MemberNo, string Color, string OriginalNames, string SaveNames)
    {
      DacMemo dacMemo = new DacMemo();
      int result = dacMemo.InsertMemo(Title, Contents, Color, MemberNo, OriginalNames, SaveNames);

      if (result > 0)
      {
        return dacMemo.SelectMemoIdx(MemberNo);
      }
      else
      {
        return 0;
      }
    }

    #endregion

    #region -메모리스트 가져오기
    /// <summary>
    /// 메모리스트 가져오기
    /// </summary>
    /// <param name="MemberNo">사번</param>
    /// <param name="Type">타입</param>
    /// <param name="Search">검색어</param>
    /// <returns></returns>
    public List<Memo> GetMemoListByCondition(string MemberNo, string Type, string Search)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.SelectMemoListByCondition(MemberNo, Type, Search);
    }

    #endregion

    #region-메모디테일 가져오기
    public Memo GetMemoDetailByIdx(int Idx)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.SelectMemoDetailByIdx(Idx);
    }

    #endregion

    #region -메모수정
    /// <summary>
    /// 메모수정
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <param name="Title">제목</param>
    /// <param name="Contents">내용</param>
    /// <param name="Color">메모지컬러</param>
    /// <param name="MemberNo">회원번호</param>
    /// <param name="OriginalNames">이미지이름</param>
    /// <param name="SaveNames">저장될 이미지이름</param>
    /// <returns></returns>
    public int EditMemo(int MemoID, string Title, string Contents, string Color, int MemberNo, string OriginalNames, string SaveNames)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.UpdateMemoDetail(MemoID, Title, Contents, Color, MemberNo, OriginalNames, SaveNames);
    }

    #endregion

    #region -메모 중요보관함에 저장
    /// <summary>
    /// 메모 중요보관함에 저장
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <returns></returns>
    public int SetImportantMemo(int MemoID)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.UpdateImportantMemo(MemoID);
    }

    #endregion

    #region -메모 삭제
    /// <summary>
    /// 메모 삭제
    /// </summary>
    /// <param name="MemoID">메모 아이디</param>
    /// <returns></returns>
    public List<ImageFile> RemoveMemo(int MemoID)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.DeleteMemo(MemoID);
    }

    #endregion

    #region -메모 원위치로 복원
    /// <summary>
    /// 메모 원위치로 복원
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <returns></returns>
    public int ReturnMemo(int MemoID)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.UpdateReturnMemo(MemoID);
    }

    #endregion

    #region -메모 순번 저장
    /// <summary>
    /// 메모 순번 저장
    /// </summary>
    /// <param name="MemoIDList">메모 아이디 리스트</param>
    /// <param name="OrderList">메모 순번 리스트</param>
    /// <returns></returns>
    public int SetOrder(string MemoIDList, string OrderList)
    {
      DacMemo dacMemo = new DacMemo();
      return dacMemo.UpdateOrderMemo(MemoIDList, OrderList);
    }
    #endregion
  }
}