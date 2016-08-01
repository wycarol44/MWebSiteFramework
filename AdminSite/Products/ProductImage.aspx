<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="ProductImage.aspx.vb" Inherits="Products_ProductImage"  EnableEventValidation="true"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" Runat="Server">

    <asp:Panel ID="Panel1" runat="server" CssClass="detail-header">
        <miles:MilesTabStrip ID="rdTabs" runat="server" ShowInvisible="true"   XmlFileName="~/App_Data/Tabs/ProductTab.xml" />
    </asp:Panel>

       
    <asp:Panel ID="Panel2" runat="server" >

             <asp:Label ID="lblImageName" runat="server" Text="Image Name" AssociatedControlID="txtImageName" />
			 <asp:Label ID="lblrImageName" runat="server" SkinID="Required" />
			 <asp:TextBox ID="txtImageName" runat="server" width="200px"/>
			 <asp:RequiredFieldValidator ID="rfvImageName" runat="server" ControlToValidate="txtImageName" ErrorMessage="Image Name is required" />

            <br />
            <miles:MilesPictureUpload ID="picUpload" runat ="server" width="300px"/>

<%--           <telerik:RadAsyncUpload runat="server" ID="radUpload" TargetFolder="C:\Webapps\TrainingLLQ\Documents\Pictures\"/>--%>
            <br />
            <miles:MilesButton ID="btnUpload" runat="server"  Text="Upload" AutoPostBack="true" ></miles:MilesButton>

    </asp:Panel>
    

    <asp:Repeater runat="server" ID ="repeater">


        <ItemTemplate>
            <style type="text/css" >
                table, th, td {
                    border: 1px solid black;
                    border-collapse: collapse;
                }

            </style>
            <td><strong><asp:Label ID="lblPrimary" runat ="server" Text="Primary Image" Visible ="false" ></asp:Label></strong></td> 
            <table>
                <tr>
 
               </tr>
                    <tr>
                        <td><asp:Label ID="lblImg" runat="server" Text='<%# Eval("ImageName") %>'></asp:Label></td>
                    </tr>
                    <tr>
                         <td><asp:HyperLink ID="Img" runat ="server" ImageUrl =<%# String.Format("~/Documents/Pictures/{0}", Eval("ThumbnailPath"))%> NavigateUrl =<%# String.Format("~/Products/FullSizeImage.aspx?PictureID={0}", Eval("PictureID"))%>  ></asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>
                          <asp:Button ID="lbtPri" runat="server" Text="Make Primary" CommandName ="Primary" CausesValidation ="false"/>
                         <%-- <asp:LinkButton ID="lbtPri" runat="server" CommandName ="Primary" Text="Set Primary" CausesValidation ="false" ></asp:LinkButton>--%>
                          <asp:LinkButton ID="lbtnDe" runat="server" CommandName ="Delete" Text="Delete" CausesValidation ="false" OnClientClick="if(!confirm('Are you sure you want to delete this Picture?')){return false;}" ></asp:LinkButton>
                        </td>
                    </tr>

            </table>

            <asp:Label ID="lblPicImgID" runat="server" Text='<%# Eval("ProductImageID")%>' Visible ="false" ></asp:Label>

        </ItemTemplate>


    </asp:Repeater> 

</asp:Content>

