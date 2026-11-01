'由于包含加解密等安全信息，本文件中的部分代码已被删除

Friend Module ModSecret

    '标注 PCL 的不同分支，仅用于替换标记
    Public Const VersionBranchMain As String = "OpenSource"
    '在开源版的注册表与常规版的注册表隔离，以防数据冲突
    Public Const RegFolder As String = "PCLDebug"
    '用于微软登录的 ClientId
    Public OAuthClientId As String = If(Environment.GetEnvironmentVariable("PCL_MS_CLIENT_ID"), "")
    'CurseForge API Key
    Public CurseForgeAPIKey As String = If(Environment.GetEnvironmentVariable("PCL_CURSEFORGE_API_KEY"), "")
    '用于匿名数据收集的腾讯云日志服务上报 URL，形如 https://{region}.cls.tencentcs.com/track?topic_id={topic_id}
    Public Const ClsBaseUrl As String = ""

#Region "网络鉴权"

    Public Const RestrictMcDownload As Boolean = False
    Friend Function SecretCdnSign(UrlWithMark As String) As String
        If Not UrlWithMark.EndsWithF("{CDN}") Then Return UrlWithMark
        Return UrlWithMark.Replace("{CDN}", "").Replace(" ", "%20")
    End Function
    ''' <summary>
    ''' 设置 Headers 的 UA、Referer。
    ''' </summary>
    Friend Sub SecretHeadersSign(Url As String, ByRef Req As HttpRequestMessage, Optional SimulateBrowserHeaders As Boolean = False)
        If Not Req.Headers.UserAgent.Any Then
            If Url.Contains("baidupcs.com") OrElse Url.Contains("baidu.com") Then
                Req.Headers.Add("User-Agent", "LogStatistic")  '#4951
            ElseIf SimulateBrowserHeaders Then
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)} Mozilla/5.0 AppleWebKit/537.36 Chrome/63.0.3239.132 Safari/537.36")
            Else
                Req.Headers.Add("User-Agent", $"PCL2/{VersionBaseName}.{CInt(BuildType)}")
            End If
        End If
        If Not SimulateBrowserHeaders Then Req.Headers.Add("Referer", $"http://{VersionCode}.open.pcl2.server/")
        If Url.Contains("api.curseforge.com") OrElse Url.Contains("forgecdn.net") Then Req.Headers.Add("x-api-key", CurseForgeAPIKey)
    End Sub

#End Region

#Region "主题"

    Public Color1 As New MyColor(52, 61, 74)
    Public Color2 As New MyColor(11, 91, 203)
    Public Color3 As New MyColor(19, 112, 243)
    Public Color4 As New MyColor(72, 144, 245)
    Public Color5 As New MyColor(150, 192, 249)
    Public Color6 As New MyColor(213, 230, 253)
    Public Color7 As New MyColor(222, 236, 253)
    Public Color8 As New MyColor(234, 242, 254)
    Public ColorBg0 As New MyColor(150, 192, 249)
    Public ColorBg1 As New MyColor(190, Color7)
    Public ColorGray1 As New MyColor(64, 64, 64)
    Public ColorGray2 As New MyColor(115, 115, 115)
    Public ColorGray3 As New MyColor(140, 140, 140)
    Public ColorGray4 As New MyColor(166, 166, 166)
    Public ColorGray5 As New MyColor(204, 204, 204)
    Public ColorGray6 As New MyColor(235, 235, 235)
    Public ColorGray7 As New MyColor(240, 240, 240)
    Public ColorGray8 As New MyColor(245, 245, 245)
    Public ColorSemiTransparent As New MyColor(1, Color8)

    Public ThemeNow As Integer = -1
    Public ColorHue As Integer = 210, ColorSat As Integer = 85, ColorLightAdjust As Integer = 0, ColorHueTopbarDelta As OneOf(Of Integer, Integer()) = 0
    Public ThemeDontClick As Integer = 0

    Public Sub ThemeRefresh(Optional NewTheme As Integer = -1)
        Try
            If ThemeNow = NewTheme AndAlso NewTheme >= 0 Then Return
            If NewTheme >= 0 Then ThemeNow = NewTheme

            Color1 = New MyColor().FromHSL2(ColorHue, ColorSat * 0.2, 25 + ColorLightAdjust * 0.3)
            Color2 = New MyColor().FromHSL2(ColorHue, ColorSat, 45 + ColorLightAdjust)
            Color3 = New MyColor().FromHSL2(ColorHue, ColorSat, 55 + ColorLightAdjust)
            Color4 = New MyColor().FromHSL2(ColorHue, ColorSat, 65 + ColorLightAdjust)
            Color5 = New MyColor().FromHSL2(ColorHue, ColorSat, 80 + ColorLightAdjust * 0.4)
            Color6 = New MyColor().FromHSL2(ColorHue, ColorSat, 91 + ColorLightAdjust * 0.1)
            Color7 = New MyColor().FromHSL2(ColorHue, ColorSat, 95)
            Color8 = New MyColor().FromHSL2(ColorHue, ColorSat, 97)
            ColorBg0 = Color4 * 0.4 + Color5 * 0.4 + ColorGray4 * 0.2
            ColorBg1 = New MyColor(190, Color7)

            ColorSemiTransparent = New MyColor(1, Color8)
            Application.Current.Resources("ColorBrush1") = New SolidColorBrush(Color1)
            Application.Current.Resources("ColorBrush2") = New SolidColorBrush(Color2)
            Application.Current.Resources("ColorBrush3") = New SolidColorBrush(Color3)
            Application.Current.Resources("ColorBrush4") = New SolidColorBrush(Color4)
            Application.Current.Resources("ColorBrush5") = New SolidColorBrush(Color5)
            Application.Current.Resources("ColorBrush6") = New SolidColorBrush(Color6)
            Application.Current.Resources("ColorBrush7") = New SolidColorBrush(Color7)
            Application.Current.Resources("ColorBrush8") = New SolidColorBrush(Color8)
            Application.Current.Resources("ColorBrushBg0") = New SolidColorBrush(ColorBg0)
            Application.Current.Resources("ColorBrushBg1") = New SolidColorBrush(ColorBg1)
            Application.Current.Resources("ColorObject1") = CType(Color1, Color)
            Application.Current.Resources("ColorObject2") = CType(Color2, Color)
            Application.Current.Resources("ColorObject3") = CType(Color3, Color)
            Application.Current.Resources("ColorObject4") = CType(Color4, Color)
            Application.Current.Resources("ColorObject5") = CType(Color5, Color)
            Application.Current.Resources("ColorObject6") = CType(Color6, Color)
            Application.Current.Resources("ColorObject7") = CType(Color7, Color)
            Application.Current.Resources("ColorObject8") = CType(Color8, Color)
            Application.Current.Resources("ColorObjectBg0") = CType(ColorBg0, Color)
            Application.Current.Resources("ColorObjectBg1") = CType(ColorBg1, Color)
            ThemeRefreshMain()
        Catch ex As Exception
            Logger.Error(ex, "刷新主题颜色失败", LogBehavior.Toast)
        End Try
    End Sub
    Public Sub ThemeRefreshMain()
        RunInUi(
        Sub()
            If Not FrmMain.IsLoaded Then Return
            '顶部条背景
            Dim Brush = New LinearGradientBrush With {.EndPoint = New Point(1, 0), .StartPoint = New Point(0, 0)}
            Dim Deltas = ColorHueTopbarDelta.Switch(Function(d) New Integer() {-d, 0, d}, Function(d) d)
            Brush.GradientStops.Add(New GradientStop With {.Offset = 0, .Color = New MyColor().FromHSL2(ColorHue + Deltas(0), ColorSat, 48 + ColorLightAdjust)})
            Brush.GradientStops.Add(New GradientStop With {.Offset = 0.5, .Color = New MyColor().FromHSL2(ColorHue + Deltas(1), ColorSat, 54 + ColorLightAdjust)})
            Brush.GradientStops.Add(New GradientStop With {.Offset = 1, .Color = New MyColor().FromHSL2(ColorHue + Deltas(2), ColorSat, 48 + ColorLightAdjust)})
            FrmMain.PanTitle.Background = Brush
            FrmMain.PanTitle.Background.Freeze()
            '主页面背景
            If Settings.Get(Of Boolean)("UiBackgroundColorful") Then
                Brush = New LinearGradientBrush With {.EndPoint = New Point(0.1, 1), .StartPoint = New Point(0.9, 0)}
                Brush.GradientStops.Add(New GradientStop With {.Offset = -0.1, .Color = New MyColor().FromHSL2(ColorHue - 15, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(New GradientStop With {.Offset = 0.4, .Color = New MyColor().FromHSL2(ColorHue, ColorSat * 0.8, 91)})
                Brush.GradientStops.Add(New GradientStop With {.Offset = 1.1, .Color = New MyColor().FromHSL2(ColorHue + 15, ColorSat * 0.8, 91)})
                FrmMain.PanForm.Background = Brush
            Else
                FrmMain.PanForm.Background = New MyColor(245, 245, 245)
            End If
            FrmMain.PanForm.Background.Freeze()
        End Sub)
    End Sub
    Friend Sub ThemeCheckAll(EffectSetup As Boolean)
    End Sub
    Friend Function ThemeCheckOne(Id As Integer) As Boolean
        Return True
    End Function
    Friend Function ThemeUnlock(Id As Integer, Optional ShowDoubleHint As Boolean = True, Optional UnlockHint As String = Nothing) As Boolean
        Return False
    End Function

#End Region

#Region "更新"

    Friend Sub UpdateCheckByButton()
        Hint("该版本中不包含更新功能……")
    End Sub

    Friend IsUpdateWaitingRestart As Boolean = False
    Public Sub UpdateRestart(TriggerRestartAndByEnd As Boolean)
    End Sub
    Public Sub UpdateReplace(ProcessId As Integer, OldFileName As String, NewFileName As String, TriggerRestart As Boolean)
    End Sub

    ''' <summary>
    ''' 确保 PathTemp/Latest.exe 是最新正式版的 PCL，它会被用于整合包打包。
    ''' 如果不是，则下载一个。
    ''' </summary>
    Friend Sub DownloadLatestPCL(Optional LoaderToSyncProgress As LoaderBase = Nothing)
        '注意：如果要自行实现这个功能，请换用另一个文件路径，以免与官方版本冲突
    End Sub

#End Region

#Region "联网配置"

    ''' <summary>
    ''' 联网获取的配置信息。
    ''' 若获取失败或仍在获取中，可能为 Nothing。
    ''' </summary>
    Public ServerConfig As JObject

    Public ServerLoader As New LoaderTask(Of Integer, Integer)("PCL 配置更新", Sub() Logger.Info("该版本中不包含更新通知功能……"), Priority:=ThreadPriority.BelowNormal) With
            {.ReloadTimeout = 1000 * 60 * 60} '超时 1 小时

#End Region

    'From https://www.bilibili.com/opus/1262353478882164752 https://ltcat.lanzouc.com/itPgs4d3bwyd
    '#Region "联网配置"

    '    ''' <summary>
    '    ''' 联网获取的配置信息。
    '    ''' 若获取失败或仍在获取中，可能为 Nothing。
    '    ''' </summary>
    '    Public ServerConfig As JObject

    '    Public ServerLoader As New LoaderTask(Of Integer, Integer)("PCL 配置更新", AddressOf ServerSub, Priority:=ThreadPriority.BelowNormal) With
    '        {.ReloadTimeout = 1000 * 60 * 60} '超时 1 小时
    '    Private Sub ServerSub(Loader As LoaderTask(Of Integer, Integer))
    '        '清理更新缓存
    '        Try
    '            If FileUtils.Exists(Paths.Base & "PCL\update.exe") Then
    '                FileUtils.Delete(Paths.Base & "PCL\update.exe")
    '                Logger.Info("已清理更新缓存")
    '            End If
    '        Catch ex As Exception
    '            Logger.Warn(ex, "清理更新缓存失败")
    '        End Try
    '        '从本地读取配置
    '        Dim LocalConfigVersion As Integer = 0
    '        Dim LocalConfigPath As String = PathTemp & "Cache\ServerConfig.json"
    '        Try
    '            If FileUtils.Exists(LocalConfigPath) Then
    '                ServerConfig = FileUtils.ReadAsJson(LocalConfigPath)
    '                LocalConfigVersion = Val(ServerConfig("Version")) '读取本地版本号
    '                Logger.Info($"已读取本地缓存的配置：{LocalConfigVersion}")
    '            End If
    '        Catch ex As Exception
    '            Logger.Warn(ex, "读取配置信息失败")
    '            ServerConfig = Nothing
    '            FileUtils.Delete(LocalConfigPath)
    '        End Try
    '        Loader.Progress = 0.2
    '        '获取公告编号
    '        Dim LocalId As Integer = Settings.Get(Of Integer)("HintNotice"), ServerId As Integer '服务器公告
    '        Dim ServerContent As String
    '        Dim NoticeFileAddress As String = PathTemp & "Cache\Notice.cfg"
    '        Try
    '            ServerContent = NetRequestByClientRetry("https://pcl2-server-1253424809.file.myqcloud.com/notice.cfg{CDN}")
    '            ServerId = Val(ServerContent.Split("|")(0))
    '            If ServerId = 0 Then Throw New Exception("获取到的内容有误！（" & ServerContent & "）")
    '            If ServerId > LocalId Then
    '                Logger.Info($"本地公告编号：{LocalId}，服务器公告：{ServerContent}，需更新")
    '            Else
    '                Logger.Info($"本地公告编号：{LocalId}，服务器公告：{ServerContent}，无需更新")
    '                '将自身复制为最新正式版
    '#If RELEASE Then
    '                If Settings.Get(Of Integer)("SystemHighestSavedBetaVersionReg") < VersionCode OrElse Not FileUtils.Exists(PathTemp & "Latest.exe") Then
    '                    Settings.Set("SystemHighestSavedBetaVersionReg", VersionCode)
    '                    Logger.Info($"复制自身为最新正式版：版本号升高到 {VersionCode}")
    '                    Try
    '                        FileUtils.Copy(PathExe, PathTemp & "Latest.exe")
    '                    Catch ex As Exception
    '                        Logger.Warn(ex, "复制自身为最新正式版失败")
    '                    End Try
    '                End If
    '#End If
    '            End If
    '            FileUtils.Write(NoticeFileAddress, ServerContent)
    '        Catch ex As Exception
    '            If TypeOf ex Is InvalidOperationException AndAlso ex.Message.Contains("FIPS") Then
    '                If MyMsgBox("由于系统未启用 FIPS 兼容算法，PCL 可能无法正常运行。" & vbCrLf &
    '                            "请按照教程启用该功能，然后重启 PCL。", "兼容性警告", "打开教程", "取消", IsWarn:=True) = 1 Then
    '                    OpenWebsite("https://blog.csdn.net/qq_37608398/article/details/81209922")
    '                End If
    '            Else
    '                Logger.Warn(ex, "获取 PCL 服务器状态失败")
    '                FileUtils.Delete(NoticeFileAddress)
    '            End If
    '            Return
    '        End Try
    '        Loader.Progress = 0.4
    '        '下载配置信息
    '        Try
    '            '如果远端版本号更高，则下载最新配置
    '            Dim ServerConfigVersion As Double = Val(ServerContent.Split("|")(4))
    '            If LocalConfigVersion < ServerConfigVersion Then
    '                Logger.Info($"本地配置版本号：{LocalConfigVersion}，服务器配置版本号：{ServerConfigVersion}，需要更新")
    '                NetDownloadByClient("https://pcl2-server-1253424809.file.myqcloud.com/ServerConfig.json{CDN}", LocalConfigPath)
    '                ServerConfig = FileUtils.ReadAsJson(LocalConfigPath)
    '            End If
    '        Catch ex As Exception
    '            Logger.Warn(ex, "下载配置信息失败")
    '        End Try
    '        Loader.Progress = 0.6
    '        '下载公告
    '        Dim NoticeContent As String
    '        Try
    '            If ServerId <= LocalId AndAlso FileUtils.Exists(PathTemp & "Cache\Notice.json") Then
    '                '无需更新，缓存已存在，使用缓存
    '                NoticeContent = FileUtils.ReadAsString(PathTemp & "Cache\Notice.json")
    '            Else
    '                '下载并保存缓存
    '                NoticeContent = NetRequestByClientRetry("https://pcl2-server-1253424809.file.myqcloud.com/notice.json{CDN}", RequireJson:=True)
    '                FileUtils.Write(PathTemp & "Cache\Notice.json", NoticeContent)
    '            End If
    '            Settings.Set("HintNotice", ServerId) '更新公告记录
    '        Catch ex As Exception
    '            Logger.Warn(ex, "下载 PCL 服务器公告失败")
    '            FileUtils.Delete(PathTemp & "Cache\Notice.json")
    '            Return
    '        End Try
    '        Loader.Progress = 0.9
    '        '读取公告
    '        Try
    '            For Each Notice As JObject In NoticeContent.Replace("{UNIQUE}", Identify).DeserializeJson()
    '                '确定 ID 比当前大
    '                Dim NoticeId As Integer = Val(If(Notice("id"), 0))
    '                If NoticeId <= LocalId Then Continue For
    '                '确保满足前置条件
    '                Dim IsAllowed As Boolean = True
    '                For Each Require As JProperty In If(Notice("requirements"), {})
    '                    If Not ServerRequirement(Require.Name, Require.Value) Then IsAllowed = False
    '                Next
    '                If Not IsAllowed Then Continue For
    '                '获取弹窗信息
    '                Dim ImportantLevel As Integer = If(Notice("importantLevel"), 2)
    '                Dim IsUpdateNotice As Boolean = If(Notice("isUpdate"), False) '是否为更新公告
    '                Logger.Info($"重要等级 {ImportantLevel}，更新公告 {IsUpdateNotice}")
    '                If Settings.Get(Of Integer)(If(IsUpdateNotice, "SystemSystemUpdate", "SystemSystemActivity")) > ImportantLevel Then Continue For
    '                If IsUpdateNotice AndAlso IsUpdating Then Continue For
    '                Dim Title As String = If(Notice("title"), "公告").ToString
    '                Dim Description As String = If(Notice("description"), "").ToString.Replace("\n", vbCrLf)
    '                Dim Buttons As JArray = If(Notice("buttons"), New JArray)
    '                Dim ButtonCount As Integer = Buttons.Count
    '                Dim Actions As Object = {}
    '                If ButtonCount > 0 Then
    '                    Logger.Info($"显示公告 {NoticeId}：{Description}")
    '                    Dim MsgBoxResult As Integer
    '                    Select Case ButtonCount
    '                        Case 1
    '                            MsgBoxResult = MyMsgBox(Description, Title, If(Buttons(0)("text"), "确定").ToString, ForceWait:=True)
    '                        Case 2
    '                            MsgBoxResult = MyMsgBox(Description, Title, If(Buttons(0)("text"), "确定").ToString, If(Buttons(1)("text"), "确定").ToString)
    '                        Case 3
    '                            MsgBoxResult = MyMsgBox(Description, Title, If(Buttons(0)("text"), "确定").ToString, If(Buttons(1)("text"), "确定").ToString, If(Buttons(2)("text"), "确定").ToString)
    '                        Case Else
    '                            Logger.Warn($"公告 {NoticeId} 的弹窗有 {ButtonCount} 个按钮，无法显示")
    '                            Continue For
    '                    End Select
    '                    MsgBoxResult = MsgBoxResult.Clamp(1, ButtonCount)
    '                    Actions = If(Buttons(MsgBoxResult - 1)("actions"), {})
    '                Else
    '                    Actions = If(Notice("actions"), {})
    '                End If
    '                '执行事件
    '                For Each Action As JProperty In Actions
    '                    Try
    '                        Select Case Action.Name
    '                            Case "copy"
    '                                ClipboardSet(Action.Value.ToString)
    '                            Case "website"
    '                                OpenWebsite(Action.Value.ToString)
    '                            Case "stop"
    '                                FrmMain.EndProgram(False)
    '                            Case "setup" '仅支持 >= 388
    '                                Dim Values As JObject = Action.Value
    '                                For Each Value In Values
    '                                    Settings.Set(Value.Key, Value.Value.ToString)
    '                                Next
    '                            Case "update", "slientupdate" '>= 392 起，不再使用 UpdateInfo 中的 url 参数，而是直接从 ServerConfig.json 读取
    '                                Dim UpdateInfo As JObject = Action.Value
    '                                Dim Silent As Boolean = Action.Name = "slientupdate"
    '                                If Not HasUpdatePermission Then
    '                                    If Not Silent Then InputPotatoCode(True)
    '                                    If Not HasUpdatePermission Then Continue For
    '                                End If
    '                                UpdateStart(Silent)
    '                            Case Else
    '                                Throw New Exception("未知的行动支：" & Action.Name & ", " & Action.Value.ToString)
    '                        End Select
    '                    Catch ex As Exception
    '                        Logger.Error(ex, $"执行 PCL 服务器公告动作失败（{Action.Name}, {Action.Value}）", LogBehavior.Toast)
    '                    End Try
    '                Next
    '            Next
    '        Catch ex As Exception
    '            Logger.Warn(NoticeContent)
    '            Logger.Error(ex, $"读取 PCL 服务器公告失败（{NoticeContent.Length}）", LogBehavior.Toast)
    '            FileUtils.Delete(PathTemp & "Cache\Notice.json")
    '        End Try
    '    End Sub

    '#End Region

#Region "赞助等级"

    Public ReadOnly Property CurrentRank As DonationRank
        Get
            Return DonationRank.None
        End Get
    End Property

    Public Sub InputPotatoCode(IsUpdating As Boolean)
    End Sub
    Friend Sub GeneratePotatoCode()
    End Sub

    ''' <summary>
    ''' 获取设备识别码。
    ''' </summary>
    Friend Function GetIdentify() As String
        Return "0000-0000-0000-0000"
    End Function

#End Region

End Module
