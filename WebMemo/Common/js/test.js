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

    if (PhoneValidate($('#txtPhone').val()) == true) {
      hfRegInfo.val(
                     $('#txtRegEmail').val() + "," +
                     $('#txtRegPassword').val() + "," +
                     $('#txtRegName').val() + "," +
                     $('#txtPhone').val()
                  );

      btnReg.click();
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