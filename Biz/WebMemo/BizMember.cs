using Dac.Entity;
using Dac.WebMemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Biz.WebMemo
{
  public class BizMember
  {
    #region -멤버 리스트 조회
    /// <summary>
    /// 멤버 리스트 조회
    /// </summary>
    /// <returns>멤버 리스트</returns>
    public List<Member> GetMemberList()
    {
      DacMember dacMember = new DacMember();
      return dacMember.SelectMemberList();
    }

    #endregion

    #region -멤버 아이디 체크(로그인)
    /// <summary>
    /// 멤버 아이디 체크(로그인)
    /// </summary>
    /// <param name="Email">이메일</param>
    /// <returns></returns>
    public int CheckId(string Email)
    {
      DacMember dacMember = new DacMember();
      return dacMember.SelectMemeberIDCheck(Email);
    }

    #endregion

    #region -멤버 비밀번호 체크(로그인)
    /// <summary>
    /// 멤버 비밀번호 체크(로그인)
    /// </summary>
    /// <param name="Email">이메일</param>
    /// <param name="Password">비밀번호</param>
    /// <returns></returns>
    public Member CheckPassword(string Email, string Password)
    {
      DacMember dacMember = new DacMember();
      return dacMember.SelectMemberPassword(Email, Password);
    }

    #endregion

    #region-멤버 아이디 조회(Email)
    /// <summary>
    /// 멤버 아이디 조회(Email)
    /// </summary>
    /// <param name="Name">이름</param>
    /// <param name="Phone">핸드폰번호</param>
    /// <returns>이메일</returns>
    public string FindID(string Name, string Phone)
    {
      DacMember dacMember = new DacMember();
      return dacMember.SelectMemberID(Name, Phone);
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
    public int FindPassword(string Email, string Name, string Password)
    {
      DacMember dacMember = new DacMember();
      return dacMember.UpdateMemberPassword(Email, Name, Password);
    }

    #endregion

    #region -멤버저장
    public string AddMember(string Email, string Password, string Name, string Phone)
    {
      DacMember dacMember = new DacMember();

      string alertText = String.Empty;

      int inserted = dacMember.InsertMember(Email, Password, Name, Phone);

      if (inserted > 0)
      {
        alertText = "회원가입 되었습니다.";
      }
      else
      {
        alertText = "회원가입에 실패하였습니다. \n\n 다시 시도하여 주십시오.";
      }

      return alertText;
    }
    #endregion

    #region -멤버 정보 조회
    /// <summary>
    /// 멤버 정보 조회
    /// </summary>
    /// <param name="MemberNo">회원번호</param>
    /// <returns></returns>
    public string GetMemberInfoById(string MemberNo)
    {
      DacMember dacMember = new DacMember();
      return dacMember.SelectMemberInfo(MemberNo);
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
      DacMember dacMember = new DacMember();
      return dacMember.UpdateMemberInfo(MemberNo, Password, Phone);
    }

    #endregion

    #region -멤버삭제
    /// <summary>
    /// 멤버삭제
    /// </summary>
    /// <param name="No">회원번호</param>
    /// <returns></returns>
    public int RemoveMember(string No)
    {
      DacMember dacMember = new DacMember();
      return dacMember.DeleteMember(No);
    }

    #endregion
  }
}