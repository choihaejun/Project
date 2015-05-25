<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MemoList.aspx.cs" Inherits="Memo_MemoList" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>WebMemo</title>
  <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>
  <script src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/js/bootstrap.min.js"></script>
  <script src="/Common/js/jquery.gridly.js"></script>
  <script src="/Common/js/syronex-colorpicker.js"></script>
  <link href="/Common/Style/syronex-colorpicker.css" rel="stylesheet" />
  <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" />
  <link href="/Common/Style/gridly.css" rel="stylesheet" />
  <link href="/Common/Style/slideMenu.css" rel="stylesheet" />
  <link href="/Common/Style/memoStyle.css" rel="stylesheet" />

  <script>
    //로그아웃
    function Logout() {
      $('body').fadeOut(1000, function () {
        window.location.href = "/Public/Login.aspx";
      });
    }

    //이미지 미리보기
    function showImagePreview(input) {
      var fileSize = input.files[0].size;
      var fileName = $('#imgFileUpload').val().replace(/C:\\fakepath\\/i, '');
      var fileType = $('#imgFileUpload').val().substring($('#imgFileUpload').val().lastIndexOf(".") + 1, $('#imgFileUpload').val().length);

      if (fileSize > 400000) {
        alert("400KB 미만의 파일만 등록이 가능합니다.");
      }
      else if (input.files && input.files[0]) {
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
          $(".modal-body").html($(".modal-body").html() + "<img id='" + $.now() + "." + fileType + "' src='" + e.target.result + "' alt='" + fileName + "'>");
        }
        filerdr.readAsDataURL(input.files[0]);
      }
      input.value = null;
    }

    //컬러픽커
    function ColorPicker(selected) {
      $('#colorPicker').colorPicker({
        defaultColor: selected,
        columns: 7,
        color: ['#EEEEEE', '#FFFF88', '#FF7400', '#CDEB8B', '#6BBA70', '#006E2E', '#4096EE'],
        click: function (color) { $('.modal-content').css("background-color", color); }
      });
    }

    //컬러픽커 default컬러 셋팅
    function SelectedColorIndex(RGBCode)
    {
      var index = 0;
      if(RGBCode == 'rgb(238, 238, 238)')
      {
        index = 0;
      }
      else if (RGBCode == 'rgb(255, 255, 136)') {
        index = 1;
      }
      else if (RGBCode == 'rgb(255, 116, 0)') {
        index = 2;
      }
      else if (RGBCode == 'rgb(205, 235, 139)') {
        index = 3;
      }
      else if (RGBCode == 'rgb(107, 186, 112)') {
        index = 4;
      }
      else if (RGBCode == 'rgb(0, 110, 46)') {
        index = 5;
      }
      else if (RGBCode == 'rgb(64, 150, 238)') {
        index = 6;
      }
      return index;
    }

    //메모장들 레이아웃
    function MemoLayout()
    {
      $('#tmpMemo').find('.brick').each(function (i) {
        $(this).css('text-overflow', 'ellipsis').css('white-space', 'nowrap').css('overflow', 'hidden').css('height', '300px');
        var cnt = 0;
        $(this).find('img').each(function (i) {
          $(this).remove();
          cnt += (i + 1);
        });

        if ($(this).find('.contents').html() == '' && cnt > 0) {
          $(this).find('.contents').text('Exsist_Image');
        }
      });
    }

    //상태에 따른 메모리스트 조회(검색어포함)
    function MemoListbyCondition(SearchStr)
    {
      $('.gridly').html('');

      $.ajax({
        type: 'POST',
        url: 'WebMemoService.asmx/MemoList',
        data: 'MemberNo=' + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + '&Type=' + $('#hfType').val() + '&Search=' + SearchStr,
        dataType: "html",
        success: function (data) {
          $('#tmpMemo').html(data);

          MemoLayout();

          $('#tmpMemo').find('.brick').each(function (i) {
            var top = Math.floor(i / 4) * 320;
            var left = (i % 4) * 240;
            $(this).css('top', top).css('left', left);
          });

          $('.gridly').append($('#tmpMemo').html());
        },
        error: function (ex) {
          alert("error" + ex);
        }
      });
    }

    //메모장 default 셋팅
    function MemoDefaultSetting(selectedColor)
    {
      ColorPicker(selectedColor);

      $(".modal-title").on("keydown", function (e) {
        var length = $(this).text().length;

        if (length > 99) {
          if (e.which != 8 && e.which != 37
              && e.which != 38 && e.which != 39
              && e.which != 40 && e.which != 46) {
            e.preventDefault();
          }
        }
      });
      
      //placeholder기능
      $(".modal-body").on("focus", function () {
        if ($(".modal-body").html() == "<font>Content</font>") {
          $(".modal-body").html("");
        }
      });
      $(".modal-body").on("focusout", function () {
        if ($(".modal-body").html().trim() == "") {
          $(".modal-body").html("<font>Content</font>");
        }
      });
      $(".modal-title").on("focus", function () {
        if ($(".modal-title").html() == "<font>Title</font>") {
          $(".modal-title").html("");
        }
      });
      $(".modal-title").on("focusout", function () {
        if ($(".modal-title").html().trim() == "") {
          $(".modal-title").html("<font>Title</font>");
        }
      });

      $("#uploadImage").on("click", function () {
        $("#imgFileUpload").click();
      });
    }

    //메모장 순번 저장
    function OrderMemo()
    {
      var memoList = '';
      var orderList = '';

      $('.gridly').find('.brick').each(function () {
        var top = Math.floor(Number($(this).css('top').toString().slice(0, -2)) / 320) * 4;
        var left = Math.floor(Number($(this).css('left').toString().slice(0, -2)) / 240);

        if (orderList == '') {
          orderList += (top + left) + 1;
          memoList += $(this).find(':hidden').val();
        }
        else {
          orderList += ',' + Number((top + left) + 1);
          memoList += ',' + $(this).find(':hidden').val();
        }
      });

      $.ajax({
        type: 'POST',
        url: 'WebMemoService.asmx/OrderMemo',
        dataType: 'html',
        data: 'MemoIDList=' + memoList + '&OrderList=' + orderList,
        success: function (data) {
        },
        error: function (ex) {
        }
      });
    }

    $(document).ready(function ()
    {
      MemoListbyCondition('');

      $('.gridly').gridly();

      //검색이벤트
      $('#btnSerch').on('click', function () {
        MemoListbyCondition($('#txtSearch').val());
      });
      $('#txtSearch').on('keypress', function (e) {
        if (e.which == 13)
        {
          e.preventDefault();
          $('#btnSerch').click();
        }
      });

      //메뉴버튼 이벤트
      $("#sideMenu").on('click',function (e) {
        e.preventDefault();
        $("#cbp-spmenu-s1").toggleClass('cbp-spmenu-open');
      });

      $("#btnSideClose").on('click', function (e) {
        e.preventDefault();
        $("#cbp-spmenu-s1").toggleClass('cbp-spmenu-open');
      });

      $('.mainMemo').on('click', function (e) {
        e.preventDefault();
        $('.content h3').text('Memo');
        OrderMemo();
        $('#hfType').val('M');
        MemoListbyCondition('');
      });

      $('.important').on('click', function (e) {
        e.preventDefault();
        $('.content h3').text('Important');
        OrderMemo();
        $('#hfType').val('I');
        MemoListbyCondition('');
      });

      $('.garbage').on('click', function (e) {
        e.preventDefault();
        $('.content h3').text('Garbage');
        OrderMemo();
        $('#hfType').val('G');
        MemoListbyCondition('');
      });
      
      $('.memberInfo').on('click', function (e) {
        e.preventDefault();
        $.ajax({
          type: 'POST',
          url: '/Public/MemberService.asmx/MemberInfoModal',
          data: 'MemberNo=' + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + '&MemberName=' + '<%= Request.Cookies["LoginInfo"]["NAME"] %>',
          dataType: 'html',
          success: function (data)
          {
            $('#MemoModal').html('');
            $('#MemoModal').html(data);
            $('#MemoModal').modal('show');

            $('#btnEditInfo').on('click', function () {
              if ($('#txtRegPwConfirm').val() != $('#txtRegPassword').val()) {
                alert('비밀번호가 맞지 않습니다.');
                return false;
              }

              if ($('#txtPhone').val() == '')
              {
                alert('전화번호를 입력해주세요.');
                return false;
              }
              
              var rgEx = /[01](0|1|6|7|8|9)[-](\d{4}|\d{3})[-]\d{4}$/g;
              var chkFlg = rgEx.test($('#txtPhone').val());
              if (!chkFlg) {
                alert('올바른 휴대폰번호가 아닙니다. '-'을 포함하여 주세요.');
                return false;  
              }
              
              $.ajax({
                type: 'POST',
                url: '/Public/MemberService.asmx/EditMemberInfo',
                data: 'MemberNo=' + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + '&Password=' + $('#txtRegPassword').val() + '&Phone=' + $('#txtPhone').val(),
                dataType: 'html',
                success: function (data) {
                  alert(data);
                  $('#MemoModal').modal('hide');
                },
                error: function (ex) {
                  alert('error : ' + ex);
                }
              });

            });
          },
          error: function (ex)
          {
            alert('error : ' + ex);
          }
        });
      });

      //메모작성버튼 이벤트
      $('#btnAddMemo').on('click', function () {
        $.ajax({
          type: 'POST',
          url: 'WebMemoService.asmx/WriteMemoModal',
          dataType: 'html',
          success: function (data) {
            $('#MemoModal').html('');
            $('#MemoModal').html(data);

            MemoDefaultSetting(0);

            $('#MemoModal').modal('show');

            $("#btnWrite").on("click", function (e) {
              e.preventDefault();

              var files = [];
              $(".modal-body").find("img").each(function (i) {
                var file = {};
                file["SaveName"] = $(this).attr("id");
                file["OriginalName"] = $(this).attr("alt");
                file["Src"] = $(this).attr("src");
                files[i] = file;
              });

              var title = $('.modal-title').html() == "<font>Title</font>" ? "" : $('.modal-title').html();

              var content = $('.modal-body').html() == "<font>Content</font>" ? "" : $('.modal-body').html();

              if (content == '') {
                alert("내용을 입력해주세요.");
                return false;
              }
              else {
                content = escape(content);
              }

              var color = $('.modal-content').css("background-color");

              var Memo = "{ Title:'" + title + "', Content:'" + content + "', Color:'" + color + "', Images:" + JSON.stringify(files) + ", Reg_ID:" + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + "}";

              $.ajax({
                type: 'POST',
                url: 'WebMemoService.asmx/Write',
                data: '{ memo : ' + Memo + '}',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                  $('#tmpMemo').html(data.d);

                  MemoLayout();

                  var brickCnt = $('.gridly').children('.brick').length;
                  var top = Math.floor((brickCnt / 4)) * 320;
                  var left = (brickCnt % 4) * 240;

                  $('#tmpMemo').children('.brick').css('top', top).css('left', left);

                  $('.gridly').append($('#tmpMemo').html());
                  $('#MemoModal').modal('hide');
                },
                error: function (ex) {
                  alert("error" + ex);
                }
              }); 
            }); 
          },
          error: function (ex) {
            alert("error" + ex);
          }
        });
      });

      //메모 상세 보기
      $(document).on('click', '.click', function () {

        var memoId = $(this).parent().find(':hidden').val();

        $.ajax({
          type: 'POST',
          url: 'WebMemoService.asmx/ViewMemoModal',
          dataType: 'html',
          data: 'Idx=' + memoId,
          success: function (data) {
            $('#tmpMemo').html('');
            $('#tmpMemo').html(data);
            $('#tmpMemo').find('.modal-body img').each(function (i) {
              $(this).attr('src', '/SaveImages/' + $(this).attr('id'));
            });
            
            $('#MemoModal').html('');
            $('#MemoModal').html($('#tmpMemo').html());
            $('#tmpMemo').html('');

            MemoDefaultSetting(SelectedColorIndex($('#MemoModal').find('.modal-content').css('background-color')));

            $('#MemoModal').modal('show');

            //수정버튼 이벤트
            $("#btnEdit").on("click", function (e) {
              e.preventDefault();

              var files = [];

              $(".modal-body").find("img").each(function (i) {
                var file = {};
                file["SaveName"] = $(this).attr("id");
                file["OriginalName"] = $(this).attr("alt");
                file["Src"] = $(this).attr("src");
                files[i] = file;
              });

              var title = $('.modal-title').html() == "<font>Title</font>" ? "" : $('.modal-title').html();

              var content = $('.modal-body').html() == "<font>Content</font>" ? "" : $('.modal-body').html();

              if (content == '') {
                alert("내용을 입력해주세요.");
                return false;
              }
              else {
                content = escape(content);
              }

              var color = $('.modal-content').css("background-color");
              var Memo = "{ ID:" + Number(memoId) + ", Title:'" + title + "', Content:'" + content + "', Color:'" + color + "', Images:" + JSON.stringify(files) + ", Reg_ID:" + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + "}";

              $.ajax({
                type: 'POST',
                url: 'WebMemoService.asmx/Edit',
                data: '{ memo : ' + Memo + '}',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                  //alert창 제외
                  MemoListbyCondition('');
                  $('#MemoModal').modal('hide');
                },
                error: function (ex) {
                  alert("error" + ex);
                }
              });
            });

            //중요보관함 이벤트
            $('#btnImportant').on('click', function () {
              $.ajax({
                type: 'POST',
                url: 'WebMemoService.asmx/SetImportant',
                data: 'MemoID=' + Number(memoId),
                success: function (data) {
                  $('#MemoModal').modal('hide');
                  MemoListbyCondition('');
                },
                error: function (ex) {
                  alert("error" + ex);
                }
              });
            });

            //삭제버튼 이벤트
            $('#btnDelete').on('click', function () {
              $.ajax({
                type: 'POST',
                url: 'WebMemoService.asmx/Remove',
                data: 'MemoID=' + Number(memoId),
                success: function (data) {
                  $('#MemoModal').modal('hide');
                  MemoListbyCondition('');
                },
                error: function (ex) {
                  alert("error" + ex);
                }
              });
            });

            //복원이벤트
            $('#btnReturn').on('click', function () {
              $.ajax({
                type: 'POST',
                url: 'WebMemoService.asmx/RetrunMemo',
                data: 'MemoID=' + Number(memoId),
                success: function (data) {
                  $('#MemoModal').modal('hide');
                  MemoListbyCondition('');
                },
                error: function (ex) {
                  alert("error" + ex);
                }
              });
            });
          },
          error: function (ex) {
            alert("error" + ex);
          }
        });
      });     
    }); //--------------end document.ready

    //페이지언로드시 현재 메모지 순번 저장
    window.onbeforeunload = function () {
      OrderMemo();
    };

  </script>
</head>
<body>
  <form id="form1" runat="server">
    <!-- 탑 -->
    <div>
      <nav class="navbar navbar-default navbar-fixed-top" role="navigation">
        <a href="#" id="sideMenu">Menu</a>
        <input type="text" id="txtSearch" size="100" placeholder="Please Enter A Search Term" />
        <input type="button" id="btnSerch" class="btn btn-default" value="Search" />
        <input type="button" id="btnAddMemo" class="btn btn-info" value="Write" />
        <a href="/Admin/Admin.aspx" class="btn btn-warning" runat="server" id="btnAdmin">Admin</a>
      </nav>
    </div>

    <!-- 콘텐츠 -->
    <div class='content'>
      <h3>Memo</h3>
      <section class='example'>
        <div class='gridly'>
        </div>
      </section>
    </div>
    <div id="tmpMemo" style="display: none">
    </div>

    <!-- 사이드메뉴 -->
    <nav class="cbp-spmenu cbp-spmenu-vertical cbp-spmenu-left" id="cbp-spmenu-s1">
      <input type="hidden" id="hfType" value="M"/>
      <h3>Menu</h3>
      <a href="#" class="mainMemo">메모장</a>
      <a href="#" class="important">중요보관함</a>
      <a href="#" class="garbage">휴지통</a>
      <a href="#" class="memberInfo">회원정보수정</a>
      <a href="#" class="logout" runat="server" id="btnLogout" onserverclick="btnLogout_ServerClick">로그아웃</a>
      <a href="#" id="btnSideClose" style="position: absolute; bottom: 0px; right: 0px">x</a>
    </nav>
  </form>

  <!-- MemoModal -->
  <div class="modal fade" id="MemoModal" tabindex="-1" role="dialog" aria-labelledby="MemoModalLabel" aria-hidden="true">

  </div>
  <!-- /.modal -->

</body>
</html>
