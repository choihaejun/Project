using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Dac.Entity;

namespace Dac.WebMemo
{
  public class DacMember
  {
    #region -변수
    
    SqlConnection conn = null;
    SqlTransaction trx = null;
    string conStr = WebConfigurationManager.ConnectionStrings["PingoliConStr"].ConnectionString;
    
    #endregion

    #region -생성자
    public DacMember()
    {
      conn = new SqlConnection(conStr);
    }
    
    #endregion

    #region -멤버 리스트 조회
    /// <summary>
    /// 멤버 리스트 조회
    /// </summary>
    /// <returns>멤버 리스트</returns>
    public List<Member> SelectMemberList()
    {
      List<Member> MemberList = new List<Member>();

      SqlDataReader Reader = SqlHelper.ExecuteReader(conn, "member_list");

      while (Reader.Read())
      {
        Member Member = new Member();

        Member.NO = (int)Reader["NO"];
        Member.EMAIL = Reader["EMAIL"].ToString();
        Member.PASSWORD = Reader["PASSWORD"].ToString();
        Member.NAME = Reader["NAME"].ToString();
        Member.PHONE = Reader["PHONE"].ToString();
        Member.LVL = (int)Reader["LVL"];
        Member.REG_ID = Reader["REG_ID"].ToString();
        Member.REG_DATE =Reader["REG_DATE"].ToString().Substring(0,10);
        Member.EDIT_ID = Reader["EDIT_ID"].ToString();

        if (!Reader["EDIT_DATE"].ToString().Equals(""))
        {
          Member.EDIT_DATE = Reader["EDIT_DATE"].ToString();
        }

        MemberList.Add(Member);
      }

      if (!Reader.IsClosed)
      {
        Reader.Close();
      }

      return MemberList;
    }

    #endregion

    #region -멤버 아이디 체크(로그인)
    /// <summary>
    /// 멤버 아이디 체크(로그인)
    /// </summary>
    /// <param name="EMAIL">이메일</param>
    /// <returns></returns>
    public int SelectMemeberIDCheck(string Email)
    {
      return (int)SqlHelper.ExecuteScalar(conn, "member_check_id", new SqlParameter("@EMAIL", Email));
    }

    #endregion

    #region -멤버 아이디 조회(Email)
    /// <summary>
    /// 멤버 아이디 조회(Email)
    /// </summary>
    /// <param name="Name">이름</param>
    /// <param name="Phone">핸드폰번호</param>
    /// <returns>이메일</returns>
    public string SelectMemberID(string Name, string Phone)
    {
      return (string)SqlHelper.ExecuteScalar(conn,
                                             "member_find_id",
                                             new SqlParameter("@NAME", Name),
                                             new SqlParameter("@PHONE", Phone)
                                             );
    }

    #endregion

    #region -멤버 비밀번호 체크(로그인)
    /// <summary>
    /// 멤버 비밀번호 체크(로그인)
    /// </summary>
    /// <param name="EMAIL">이메일</param>
    /// <param name="PASSWORD">비밀번호</param>
    /// <returns></returns>
    public Member SelectMemberPassword(string Email, string Password)
    {
      SqlDataReader reader = SqlHelper.ExecuteReader(conn,
                                                     "member_check_pw",
                                                     new SqlParameter("@EMAIL", Email),
                                                     new SqlParameter("@PASSWORD", Password)
                                                    );
      Member member = new Member();

      if (reader.Read())
      {
        member.NO = (int)reader["NO"];
        member.NAME = reader["NAME"].ToString();
        member.LVL = (int)reader["LVL"];
      }

      if (!reader.IsClosed)
      {
        reader.Close();
      }

      return member;
    }

    #endregion

    #region -멤버 비밀번호 찾기(새 비밀번호 저장)
    /// <summary>
    /// 멤버 비밀번호 찾기(새 비밀번호 저장)
    /// </summary>
    /// <param name="Email">이메일</param>
    /// <param name="Name">이름</param>
    /// <param name="Password">비밀번호</param>
    /// <returns></returns>
    public int UpdateMemberPassword(string Email, string Name, string Password)
    {
      int updated = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "member_update_pw",
                                            new SqlParameter("@EMIAL", Email),
                                            new SqlParameter("@NAME", Name),
                                            new SqlParameter("@PASSWORD", Password)
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

    #region -멤버저장
    /// <summary>
    /// 멤버저장
    /// </summary>
    /// <param name="Email">이메일</param>
    /// <param name="Password">패스워드</param>
    /// <param name="Name">이름</param>
    /// <param name="Question">질문</param>
    /// <param name="Answer">답변</param>
    /// <returns></returns>
    public int InsertMember(string Email, string Password, string Name, string Phone)
    {
      int inserted = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        inserted = SqlHelper.ExecuteNonQuery(trx,
                                             "member_insert",
                                             new SqlParameter("@EMIAL", Email),
                                             new SqlParameter("@PASSWORD", Password),
                                             new SqlParameter("@NAME", Name),
                                             new SqlParameter("@PHONE", Phone)
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

    #region - 멤버 정보 조회
    /// <summary>
    /// 멤버 정보 조회
    /// </summary>
    /// <param name="MemberNo">회원번호</param>
    /// <returns></returns>
    public string SelectMemberInfo(string MemberNo)
    {
      return (string)SqlHelper.ExecuteScalar(conn,
                                             "member_select_info",
                                             new SqlParameter("@MemberNo", MemberNo)
                                             );
    }

    #endregion

    #region -멤버 정보 수정
    /// <summary>
    /// 멤버 정보 수정
    /// </summary>
    /// <param name="MemberNo">회원번호</param>
    /// <param name="Password">비밀번호</param>
    /// <param name="Phone">전화번호</param>
    /// <returns></returns>
    public int UpdateMemberInfo(string MemberNo, string Password, string Phone)
    {
      int updated = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        updated = SqlHelper.ExecuteNonQuery(trx,
                                            "member_update_info",
                                            new SqlParameter("@MemberNo", MemberNo),
                                            new SqlParameter("@Password", Password),
                                            new SqlParameter("@Phone", Phone)
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

    #region -멤버삭제
    public int DeleteMember(string No)
    {
      int deleted = 0;

      try
      {
        conn.Open();

        trx = conn.BeginTransaction();

        deleted = SqlHelper.ExecuteNonQuery(trx,
                                             "member_delete",
                                             new SqlParameter("@NO", No)
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

      return deleted;

    }

    #endregion
  }
}