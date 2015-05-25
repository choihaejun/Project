using Dac.Entity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Dac.WebMemo
{
  public class DacMemo
  {
    #region -변수

    SqlConnection conn = null;
    SqlTransaction trx = null;
    string conStr = WebConfigurationManager.ConnectionStrings["PingoliConStr"].ConnectionString;

    #endregion

    #region -생성자
    public DacMemo()
    {
      conn = new SqlConnection(conStr);
    }

    #endregion

    #region -메모저장
    /// <summary>
    /// 메모저장
    /// </summary>
    /// <param name="Title">제목</param>
    /// <param name="Contents">내용</param>
    /// <param name="Color">메모지색</param>
    /// <param name="MemberNo">회원번호</param>
    /// <param name="OriginalNames">실제 이미지명</param>
    /// <param name="SaveNames">저장될 이미지명</param>
    /// <returns></returns>
    public int InsertMemo(string Title, string Contents, string Color, int MemberNo, string OriginalNames, string SaveNames)
    {
      int inserted = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        inserted = SqlHelper.ExecuteNonQuery(trx,
                                             "memo_insert",
                                             new SqlParameter("@Title", Title),
                                             new SqlParameter("@Contents", Contents),
                                             new SqlParameter("@Color", Color),
                                             new SqlParameter("@MemberNo", MemberNo),
                                             new SqlParameter("@OriginalNames", OriginalNames),
                                             new SqlParameter("@SaveNames", SaveNames)
                                            );
        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return inserted;
    }

    #endregion

    #region -저장된 메모 인덱스조회
    /// <summary>
    /// 저장된 메모 인덱스조회
    /// </summary>
    /// <param name="MemberNo">회원번호</param>
    /// <returns></returns>
    public int SelectMemoIdx(int MemberNo)
    {
      int memoIdx = 0;

      memoIdx = (int)SqlHelper.ExecuteScalar(conn,
                                              "memo_idx_select",
                                              new SqlParameter("@MemberNo", MemberNo)
                                              );

      return memoIdx;
    }

    #endregion

    #region -메모리스트 조회
    /// <summary>
    /// 메모리스트 조회
    /// </summary>
    /// <param name="MemberNo">회원번호</param>
    /// <param name="Type">타입</param>
    /// <returns>메모리스트</returns>
    public List<Memo> SelectMemoListByCondition(string MemberNo, string Type, string Search)
    {
      List<Memo> memoList = new List<Memo>();

      SqlDataReader rd = SqlHelper.ExecuteReader(conn,
                                                 "memo_list",
                                                 new SqlParameter("@MEMBER_NO", MemberNo),
                                                 new SqlParameter("@TYPE", Type),
                                                 new SqlParameter("@SEARCH", Search)
                                                );

      while(rd.Read())
      {
        Memo memo     = new Memo();
        memo.ID       = (int)rd["IDX"];
        memo.Title    = rd["TITLE"].ToString();
        memo.Content  = rd["CONTENTS"].ToString();
        memo.Color    = rd["COLOR"].ToString();

        memoList.Add(memo);
      }

      if(!rd.IsClosed)
      {
        rd.Close();
      }

      return memoList;
    }
    #endregion

    #region -메모디테일 가져오기
    /// <summary>
    /// 메모디테일 가져오기
    /// </summary>
    /// <param name="Idx">메모아이디</param>
    /// <returns></returns>
    public Memo SelectMemoDetailByIdx(int Idx)
    {
      SqlDataReader rd = SqlHelper.ExecuteReader(conn,
                                                 "memo_detail",
                                                 new SqlParameter("@IDX", Idx)
                                                );
      Memo memo = new Memo();
      if(rd.Read())
      {
        memo.ID        = (int)rd["IDX"];
        memo.Title     = rd["TITLE"].ToString();
        memo.Content   = rd["CONTENTS"].ToString();
        memo.Color     = rd["COLOR"].ToString();
        memo.Del_Flag  = rd["DEL_FLAG"].ToString();
        memo.Important = rd["IMPORTANT"].ToString();
        memo.Edit_Date = rd["EDIT_DATE"].ToString();
      }

      if (!rd.IsClosed)
      {
        rd.Close();
      }

      return memo;
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
    /// <param name="OriginalNames">이미지 이름</param>
    /// <param name="SaveNames">저장될 이미지 이름</param>
    /// <returns></returns>
    public int UpdateMemoDetail(int MemoID, string Title, string Contents, string Color, int MemberNo, string OriginalNames, string SaveNames)
    {
      int updated = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "memo_update",
                                            new SqlParameter("@MemoId", MemoID),
                                            new SqlParameter("@Title", Title),
                                            new SqlParameter("@Contents", Contents),
                                            new SqlParameter("@Color", Color),
                                            new SqlParameter("@MemberNo", MemberNo),
                                            new SqlParameter("@OriginalNames", OriginalNames),
                                            new SqlParameter("@SaveNames", SaveNames)
                                           );
        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return updated;
    }
    #endregion

    #region -메모 중요보관함에 저장
    /// <summary>
    /// 메모 중요보관함에 저장
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <returns></returns>
    public int UpdateImportantMemo(int MemoID)
    {
      int updated = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "memo_update_important",
                                            new SqlParameter("@MemoId", MemoID)
                                           );
        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return updated;
    }
    #endregion

    #region -메모 삭제
    /// <summary>
    /// 메모 삭제
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <returns></returns>
    public List<ImageFile> DeleteMemo(int MemoID)
    {
      List<ImageFile> imageList = new List<ImageFile>();
      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        SqlDataReader rd = SqlHelper.ExecuteReader(trx,
                                                   "memo_delete",
                                                   new SqlParameter("@MemoId", MemoID)
                                                  );
        while(rd.Read())
        {
          if(rd["SAVE_NAME"].ToString() == "0")
          {
            break;
          }
          else
          {
            ImageFile image = new ImageFile();
            image.SaveName = rd["SAVE_NAME"].ToString();
            imageList.Add(image);
          }
        }


        if(!rd.IsClosed)
        {
          rd.Close();
        }

        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return imageList;
    }
    #endregion

    #region -메모 원위치로 복원
    /// <summary>
    /// 메모 원위치로 복원
    /// </summary>
    /// <param name="MemoID">메모아이디</param>
    /// <returns></returns>
    public int UpdateReturnMemo(int MemoID)
    {
      int updated = 0; 
      
      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "memo_update_return",
                                            new SqlParameter("@MemoId", MemoID)
                                           );
        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return updated;
    }

    #endregion

    #region -메모 순번 저장
    /// <summary>
    /// 메모 순번 저장
    /// </summary>
    /// <param name="MemoIDList">메모 아이디 리스트</param>
    /// <param name="OrderList">메모 순번 리스트</param>
    /// <returns></returns>
    public int UpdateOrderMemo(string MemoIDList, string OrderList)
    {
      int updated = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "memo_update_order",
                                            new SqlParameter("@MemoIDList", MemoIDList),
                                            new SqlParameter("@OrderList", OrderList)
                                           );
        trx.Commit();
      }
      catch (Exception)
      {
        if (trx != null)
        {
          trx.Rollback();
        }
      }
      finally
      {
        if (conn != null)
        {
          conn.Close();
        }
      }

      return updated;
    }

    #endregion
  }
}