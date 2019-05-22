<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linkedinaccts.aspx.cs" Inherits="LinkedInAPICall.linkedinaccts" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <div runat="server" id="divError">
            ServiceErrorCode:<asp:Label runat="server" ID="lblServiceErrorCode"></asp:Label>
            Message:<asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
            Status: <asp:Label runat="server" ID="lblErrorStatus"></asp:Label>
        </div>
    </form>
</body>
</html>
