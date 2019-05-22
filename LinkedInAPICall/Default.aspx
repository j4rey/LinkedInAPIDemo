<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LinkedInAPICall._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>LinkedIn: </p>
            <p>
                <asp:LinkButton runat="server" class="btn btn-default" OnClick="Go">Go &raquo;</asp:LinkButton>
                <asp:Button runat="server" OnClick="Create" Text="Create" Visible="false"/>
            </p>
        </div>
    </div>
</asp:Content>
