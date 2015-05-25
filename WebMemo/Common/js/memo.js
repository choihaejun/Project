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

function ColorPicker(selected) {
  $('#colorPicker').colorPicker({
    defaultColor: selected,
    columns: 7,
    color: ['#EEEEEE', '#FFFF88', '#FF7400', '#CDEB8B', '#6BBA70', '#006E2E', '#4096EE'],
    click: function (color) { $('.modal-content').css("background-color", color); }
  });
}

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

  $('#btnSerch').on('click', function () {
    MemoListbyCondition($('#txtSearch').val());
  });

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
          }); //------------end ajax
        }); //-------------end wirteEvent

      },
      error: function (ex) {
        alert("error" + ex);
      }
    });
  });

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

          var Memo = "{ ID:'" + Number(memoId) + "', Title:'" + title + "', Content:'" + content + "', Color:'" + color + "', Images:" + JSON.stringify(files) + ", Reg_ID:" + '<%= Request.Cookies["LoginInfo"]["NO"] %>' + "}";

          $.ajax({
            type: 'POST',
            url: 'WebMemoService.asmx/Edit',
            data: '{ memo : ' + Memo + '}',
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
              alert(data.d);
              MemoListbyCondition('');
              $('#MemoModal').modal('hide');
            },
            error: function (ex) {
              alert("error" + ex);
            }
          }); //------------end ajax
        });

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