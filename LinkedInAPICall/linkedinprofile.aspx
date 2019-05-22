<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="linkedinprofile.aspx.cs" Inherits="LinkedInAPICall.linkedinprofile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Access Token: <asp:Label runat="server" ID="lblAccessToken"></asp:Label><br />
            Expires In: <asp:Label runat="server" ID="lblExpiresIn"></asp:Label>
        </div>
        <div>
            <asp:Button runat="server" ID="btnGetUserProfile" OnClick="btnGetUserProfile_Click" Text="Get User Profile First" />
            ID: <asp:Label runat="server" ID="lblUserID"></asp:Label>
            First Name: <asp:Label runat="server" ID="lblFirstName"></asp:Label>
            Last Name: <asp:Label runat="server" ID="lblLastName"></asp:Label>
        </div>
        <hr />
        <div>
            <h2>Get Posts</h2>
            <asp:GridView runat="server" ID="grdPosts"></asp:GridView>
            <asp:Button runat="server" ID="btnGetPosts" Text="Get UGC Posts" OnClick="btnGetPosts_Click" Visible="false" />
            <asp:Button runat="server" ID="btnGetSharePosts" Text="Get Share Posts" OnClick="btnGetSharePosts_Click" Visible="false" />
        </div>
        <div>
            <h2>Text Share</h2>
            <asp:TextBox runat="server" ID="txtTextShare" TextMode="MultiLine"></asp:TextBox>
            <asp:Button runat="server" ID="btnTextShare" Text="Share" OnClick="btnTextShare_Click" Visible="false"/>
        </div>
        <div>
            <h2>Image Share</h2>
            <asp:FileUpload runat="server" ID="fileUpload" />
            <asp:TextBox TextMode="MultiLine" MaxLength="5" runat="server" ID="txtImageText"></asp:TextBox>
            <asp:Button runat="server" ID="btnFileUpload" Text="UploadFile" OnClick="btnFileUpload_Click" Visible="false" />
        </div>

        <div>
            <h2>Get Organizations</h2>
            <asp:Button runat="server" ID="btnGetOrganizations" Text="Get Organizations" OnClick="btnGetOrganizations_Click" Visible="false" />
            <asp:GridView runat="server" ID="grdGetOrganization">
                <Columns>
                    <asp:BoundField DataField="_organizationalTarget.localizedName" HeaderText="Name" />
                </Columns>
            </asp:GridView>
        </div>

        <div runat="server" id="divError">
            ServiceErrorCode:<asp:Label runat="server" ID="lblServiceErrorCode"></asp:Label>
            Message:<asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
            Status: <asp:Label runat="server" ID="lblErrorStatus"></asp:Label>
        </div>
        <div runat="server" id="divSuccess" visible="false">
            ShareURN: <asp:Label runat="server" ID="lblShareURN"></asp:Label>
        </div>
    </form>
</body>
</html>
