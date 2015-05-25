<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin_Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.0/css/bootstrap.min.css" rel="stylesheet" />
  <title></title>
</head>
<body>
  <form id="form1" runat="server">
    <div>
      <h1 class="text-primary">Admin Page</h1><a style="position:fixed; top:0; right:0;" href="/Memo/MemoList.aspx" class="btn btn-default btn-lg">Back</a>
      <br />
      <br />
      <div class="bg-info" style="float: left; margin-bottom:100px; width:100%">
        <h3 class="text-primary">Member List</h3>
        <asp:Repeater ID="rptMemberList" runat="server">
          <HeaderTemplate>
            <table class="table table-striped">
              <tr>
                <td>NO</td>
                <td>EMAIL</td>
                <td>이름</td>
                <td>전화번호</td>
                <td>가입일</td>
                <td>삭제</td>
              </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td><%# Eval("NO") %></td>
              <td><%# Eval("EMAIL") %></td>
              <td><%# Eval("NAME") %></td>
              <td><%# Eval("PHONE") %></td>
              <td><%# Eval("REG_DATE") %></td>
              <asp:HiddenField ID="hfMemberNo" runat="server" Value='<%# Eval("NO") %>' />
              <td>
                <asp:LinkButton ID="lbtnDeleteMember" runat="server" OnClick="lbtnDeleteMember_Click">X</asp:LinkButton></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            </table>
          </FooterTemplate>
        </asp:Repeater>
      </div>
      <div class="bg-info" style="float: left; margin-bottom:100px; width:100%">
        <h3 class="text-primary">Image Management</h3>
        <p class="text-warning">시간이 많이 소요될 수 있습니다.</p>
        <button class="btn-danger" runat="server" id="btnImageDelete" onserverclick="btnImageDelete_ServerClick">이미지 정리</button>
      </div>
    </div>
  </form>
</body>
</html>
