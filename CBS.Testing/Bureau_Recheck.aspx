<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bureau_Recheck.aspx.cs" Inherits="CBS.Testing.Bureau_Recheck" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bureau Re-Check</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</head>
<body>
    <div class="container">
        <form id="re_check" class="form-horizontal" style="margin-top: 10%" runat="server">
            <div class="form-group">
                <div class="col-md-4 col-md-offset-4">
                    <label class="control-label col-md-4" for="email">App ID :</label>
                    <div class="col-md-8">
                        <input type="text" runat="server" class="form-control" id="app_id" name="app_id" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-md-offset-4">
                    <label class="control-label col-md-4" for="email">Cust Name :</label>
                    <div class="col-md-8">
                        <input type="text" runat="server" class="form-control" id="cust_name" name="cust_name" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-md-offset-4">
                    <label class="control-label col-md-4" for="email">ID Number :</label>
                    <div class="col-md-8">
                        <input type="text" runat="server" class="form-control" id="id_number" name="id_number" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-4 col-md-offset-4">
                    <div class="col-md-8 col-md-offset-4">
                        <button type="reset" runat="server" class="btn btn-danger">Reset</button>
                        <asp:Button ID="view_button" runat="server" class="btn btn-primary" Text="View" OnClick="view_button_Click"/>
                    </div>
                </div>
            </div>

            <asp:PlaceHolder ID="placeholder" runat="server" />

            <div class="right">
                <asp:Button ID="Button1" runat="server" class="btn btn-primary" Text="Generate Excel" style="float:right;" OnClick="Button1_Click"/>
            </div>
            
        </form>

        <%--<table class="table table-bordered">
            <thead>
                <tr>
                    <th>App ID</th>
                    <th>Customer Name</th>
                    <th>Receipt Date</th>
                    <th>Customer Number</th>
                    <th>Mother MN</th>
                    <th>Req</th>
                    <th>All</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>John</td>
                    <td>Doe</td>
                    <td>john@example.com</td>
                    <td>Mary</td>
                    <td>Moe</td>
                    <td>mary@example.com</td>
                    <td>mary@example.com</td>
                </tr>
                <tr>
                    <td>John</td>
                    <td>Doe</td>
                    <td>john@example.com</td>
                    <td>Mary</td>
                    <td>Moe</td>
                    <td>mary@example.com</td>
                    <td>mary@example.com</td>
                </tr>
            </tbody>
        </table>--%>
    </div>
    
</body>
</html>
