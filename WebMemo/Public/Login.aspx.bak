<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Public_Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title></title>
  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>
  <script type="text/javascript" src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js"></script>
  <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" />
  <link href="/Common/Style/style.css" rel="stylesheet" />
  <script>
    //로그인
    function Login() {
      $('body').fadeOut(1000, function () {
        window.location.href = "/Memo/MemoList.aspx";
      });
    }

    //핸드폰번호 유효성 체크
    function PhoneValidate(Phone) {
      var rgEx = /[01](0|1|6|7|8|9)[-](\d{4}|\d{3})[-]\d{4}$/g;
      var chkFlg = rgEx.test(Phone);
      if (!chkFlg) {
        alert("올바른 휴대폰번호가 아닙니다. '-'을 포함하여 주세요.");
        return false;
      }
      else {
        return true;
      }
    }

    //이메일 유효성 체크
    function EmailCheck(Email) {
      var Emailval = Email;
      var regex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/;

      if (regex.test(Emailval) === false) {
        alert("잘못된 이메일 형식입니다.");
        return false;
      }
      else {
        return true;
      }
    }

    $(document).ready(function () {
      var btnLogin = $("#<%=btnLogin.ClientID%>");          //로그인버튼
      var btnReg = $('#<%=btnReg.ClientID%>');              //회원가입버튼
      var hfRegInfo = $('#<%=hfValue.ClientID%>');          //회원정보 담을 히든필드

      //로그인 enter버튼제어
      $('#txtEmail').on('keypress', function (e) {
        if (e.which == 13) {
          e.preventDefault();
        }
      });

      //로그인버튼 이벤트
      btnLogin.on("click", function () {
        if ($('#txtEmail').val() == "") {
          alert("이메일을 입력해주세요.");
          return false;
        }
        else if ($('#txtPassword').val() == "") {
          alert("비밀번호를 입력해주세요.");
          return false;
        }
      });

      //회원가입버튼 이벤트
      $('#btnRegModal').on('click', function () {
        $('#txtEmail').val('');
        $('#txtPassword').val('');
        $('#chkRemember').removeAttr('checked');

      });

      //회원가입 이메일중복체크 이벤트
      $('#btnRegIdCheck').on('click', function () {
        if ($('#txtRegEmail').val() == '') {
          alert('이메일을 입력해주세요.');
          return false;
        }

        if (EmailCheck($('#txtRegEmail').val()) == true) {
          if ($('#btnRegIdCheck').val() == "수정") {
            $('#txtRegEmail').removeAttr("disabled");
            $('#txtRegEmail').val("");
            $('#btnRegIdCheck').val("중복체크");
            $('#emailCheckLabel').empty();
          }
          else {
            $.ajax({
              type: "POST",
              url: "EmailCheck.aspx",
              data: "email=" + $("#txtRegEmail").val(),
              success: function (data) {
                if (jQuery.trim(data) == "YES") {
                  $('#emailCheckLabel').html("<font color='red'>사용가능</font>");
                  $('#txtRegEmail').attr("disabled", "disabled");
                  $('#btnRegIdCheck').val("수정");
                }
                else {
                  $('#emailCheckLabel').html("<font color='red'>사용불가능</font>");
                }
              },
              error: function () {
                alert('Request Failed');
              }
            });
          }
        }
      });

      //회원가입버튼(모달창) 이벤트
      $('#btnRegist').on("click", function () {
        if ($('#emailCheckLabel').find("font").text() != "사용가능") {
          alert("이메일을 중복체크해주세요.");
          return false;
        }
        if ($("#txtRegPassword").val() == "") {
          alert("비밀번호를 입력해주세요.");
          return false;
        }
        if ($("#txtRegPwConfirm").val() == "") {
          alert("비밀번호확인을 입력해주세요.");
          return false;
        }
        if ($("#txtRegPwConfirm").val() != $("#txtRegPassword").val()) {
          alert("비밀번호가 맞지 않습니다.");
          return false;
        }
        if ($('#txtRegName').val() == "") {
          alert("이름을 입력해주세요");
          return false;
        }
        if ($('#txtPhone').val() == "") {
          alert("휴대폰번호를 입력해주세요");
          return false;
        }
        if ($('txtCaptcha').val() == "") {
          alert("문자를 입력해주세요.");
          return false;
        }

        //캣차기능
        var flag = true;

        $.ajax({
          type: "POST",
          url: "MemberService.asmx/CheckCaptcha",
          data: "CheckStr=" + $('#txtCaptcha').val(),
          dataType: "text",
          async: false,
          success: function (data) {
            if (data == "F") {
              $('#captchaLabel').html("<font color='red'>잘못 입력하셨습니다.</font>");
              flag = false;
            }
          },
          error: function () {
            alert('Request Failed');
          }
        });

        if (flag == false) {
          return false;
        }
        else {
          if (PhoneValidate($('#txtPhone').val()) == true) {
            hfRegInfo.val(
                           $('#txtRegEmail').val() + "," +
                           $('#txtRegPassword').val() + "," +
                           $('#txtRegName').val() + "," +
                           $('#txtPhone').val()
                        );


            btnReg.click();
          }
        }
      });

      //회원가입 모달창이 사라지고 난 후 이벤트
      $('#regModal').on('hide.bs.modal', function () {
        $('#txtRegEmail').removeAttr("disabled");       //이메일 비활성화 해제
        $('#txtRegEmail').val("");                      //이메일입력란 초기화
        $('#btnRegIdCheck').val("중복체크");             //중복체크버튼 초기화
        $('#emailCheckLabel').empty();                  //이메일체크 라벨 삭제
        $('#txtRegPassword').val("");                   //패스워드입력란 초기화
        $('#txtRegPwConfirm').val("");                  //패스워드확인란 초기화
        $('#txtRegName').val("");                       //이름입력란 초기화
        $('#txtPhone').val("");                         //전화번호란 초기화
        $('#txtCaptcha').val("");
      });

      //아이디찾기버튼 이벤트
      $("#btnFindID").on("click", function () {

        $('#inputID').empty();

        if ($("#txtFindIdName").val() == "") {
          alert("이름을 입력해주세요.");
          return false;
        }
        if ($("#txtFindIdPhone").val() == "") {
          alert("핸드폰번호를 입력해주세요.");
          return false;
        }

        if (PhoneValidate($('#txtFindIdPhone').val()) == true) {

          $.ajax({
            type: "POST",
            url: "FindInfo.aspx",
            data: "type=I&name=" + $("#txtFindIdName").val() + "&phone=" + $("#txtFindIdPhone").val(),
            success: function (data) {
              if (jQuery.trim(data) != "") {
                $('#inputID').html("아이디 : <font color='red'>" + data + "</font>");
              }
              else {
                $('#inputID').html("<font color='red'>아이디를 찾을 수 없습니다.</font>");
              }

              $("#txtFindIdName").val("");
              $("#txtFindIdPhone").val("");
            },
            error: function () {
              alert('Request Failed');
            }
          });
        }
      });

      //비밀번호찾기 버튼 이벤트
      $("#btnFindPW").on("click", function () {
        if ($("#txtFindPwEmail").val() == "") {
          alert("이메일을 입력해주세요.");
          return false;
        }

        EmailCheck($("#txtFindPwEmail").val());

        if ($("#txtFindPwName").val() == "") {
          alert("이름을 입력해주세요");
          return false;
        }

        $("#btnFindPW").val("전송중.....");

        $.ajax({
          type: "POST",
          url: "FindInfo.aspx",
          data: "type=P&email=" + $("#txtFindPwEmail").val() + "&name=" + $("#txtFindPwName").val(),
          success: function (data) {
            alert(data);
            $('#findModal').modal('hide');
          },
          error: function () {
            alert('Request Failed');
          }
        });
      });

      //아이디 / 비밀번호찾기 모달창이 사라지고 난 후 이벤트
      $('#findModal').on('hide.bs.modal', function () {
        $("#txtFindPwEmail").val("");
        $("#txtFindPwName").val("");
        $("#txtFindIdName").val("");
        $("#txtFindIdPhone").val("");
        $('#inputID').empty();
        $("#btnFindPW").val("새 비밀번호 발송");
      });
    });
  </script>
</head>
<body>
  <section class="container">
    <div class="login">
      <h1>Login to WebMemo</h1>
      <form name="LoginForm" runat="server">
        <p>
          <input type="email" name="email" id="txtEmail" class="form-control" value="" placeholder="Email" runat="server" />
        </p>
        <p>
          <input type="password" name="password" class="form-control" id="txtPassword" value="" placeholder="Password" runat="server" />
        </p>
        <p class="remember_me">
          <label>
            <input type="checkbox" name="remember_me" id="chkRemember" runat="server" />
            아이디 저장
          </label>
        </p>
        <p class="submit">
          <asp:Button ID="btnLogin" class="btn btn-default" OnClick="LoginBtn_Click" Text="Login" runat="server" />
          <asp:Button ID="btnReg" OnClick="btnReg_Click" Style="display: none" runat="server" />
          <asp:HiddenField ID="hfValue" runat="server" />
        </p>
      </form>
    </div>

    <div class="login-help">
      <p><a href="" id="btnRegModal" data-toggle="modal" data-target="#regModal">회원가입</a></p>
      <p>아이디 / 비밀번호 찾기 : <a href="" data-toggle="modal" data-target="#findModal">여기를 눌러주세요</a>.</p>
    </div>
  </section>

  <!-- regModal -->
  <div class="modal fade" id="regModal" tabindex="-1" role="dialog" aria-labelledby="regModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="btnRegClose">×</button>
          <h4 class="modal-title">회원가입</h4>
        </div>
        <form role="form">
          <div class="modal-body">
            <div class="form-group">
              <label for="inputEmail">Email address</label>
              <input type="email" class="form-control" id="txtRegEmail" maxlength="30" placeholder="Enter email" />
              <input type="button" id="btnRegIdCheck" class="btn btn-info" value="중복체크" style="margin-top:5px"/>
              <div id="emailCheckLabel" style="float: right"></div>
            </div>
            <div class="form-group">
              <label for="inputPassword">Password</label>
              <input type="password" class="form-control" id="txtRegPassword" maxlength="20" placeholder="Password" /><br />
              <input type="password" class="form-control" id="txtRegPwConfirm" maxlength="20" placeholder="PasswordConfirm" />
            </div>
            <div class="form-group">
              <label for="inputName">Name</label>
              <input type="text" class="form-control" id="txtRegName" placeholder="Name" maxlength="20" />
            </div>
            <div class="form-group">
              <label for="phone">Phone</label>
              <input type="tel" class="form-control" id="txtPhone" placeholder="Phone" maxlength="13" /><br />
            </div>
            <div class="form-group">
              <asp:Image ID="imgCaptcha" runat="server" ImageUrl="~/Captcha/CImage.aspx" />
              <input type="text" id="txtCaptcha" placeholder="Enter the Captcha" />
              <p id="captchaLabel"></p>
            </div>
          </div>
          <div class="modal-footer">
            <input type="button" class="btn btn-default" id="btnRegist" value="가입" />
          </div>
        </form>
      </div>
      <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
  </div>
  <!-- /.modal -->

  <!-- findModal -->
  <div class="modal fade" id="findModal" tabindex="-1" role="dialog" aria-labelledby="findModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal" aria-hidden="true" id="btnFindClose">×</button>
          <h4 class="modal-title">아이디 / 비밀번호찾기</h4>
        </div>
        <div class="modal-body">
          <form role="form">
            <label>아이디 찾기</label><br />
            <input type="text" maxlength="20" size="30" id="txtFindIdName" placeholder="Name" />
            <input type="text" maxlength="13" size="30" id="txtFindIdPhone" placeholder="Phone" />
            &nbsp;
                        <input type="button" class="btn btn-default" id="btnFindID" value="아이디찾기" />
            <div id="inputID"></div>
          </form>
          <hr />
          <form role="form">
            <label>비밀번호 찾기</label><br />
            <input type="email" maxlength="30" size="30" id="txtFindPwEmail" placeholder="Email" />
            <input type="text" maxlength="20" size="30" id="txtFindPwName" placeholder="Name" />
            &nbsp;
            <input type="button" class="btn btn-default" id="btnFindPW" value="새 비밀번호 발송" />
          </form>
        </div>
        <div class="modal-footer">
        </div>
      </div>
      <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
  </div>
  <!-- /.modal -->
</body>
</html>
