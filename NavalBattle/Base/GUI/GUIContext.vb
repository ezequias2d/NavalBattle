Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

#Region "FILE DESCRIPTION"
''-----------------------------------------------------------------------------
'' GUIContext.vb
''
'' Author: Ezequias Moises dos Santos Silva.
''-----------------------------------------------------------------------------
#End Region

''' <summary>
''' GUIContext, representa um painel com controles/componentes navegavel por curso fixo que pode ser aninhados.
''' 
''' Componentes são implementado a parti da herança da classe GUIObject.
''' </summary>
Public Class GUIContext
    Inherits GUIObject

    Private negativeCounter As Integer = 0

    ''' <summary>
    ''' Caixa que envolve o GUIContext para representar a seleção quando um GUIContext é um componente de um GUIContext pai.
    ''' </summary>
    ''' <returns> Caixa de seleção </returns>
    Private Property SelectBox As Box

    ''' <summary>
    ''' Controles/Componentes do GUIContext
    ''' </summary>
    ''' <returns> Controles/Componentes </returns>
    Public Property Controllers As IDictionary(Of Integer, GUIObject)

    ''' <summary>
    ''' Flag de cursor ativado
    ''' </summary>
    ''' <returns> Se cursor é visivel </returns>
    Public Property CursorEnable As Boolean

    ''' <summary>
    ''' Flag de cursor movivel
    ''' </summary>
    ''' <returns></returns>
    Public Property MovableCursor As Boolean

    ''' <summary>
    ''' Velocidade de troca de GUIObject selecionados
    ''' </summary>
    ''' <returns> Velocidade de troca de GUIObject </returns>
    Public Property SpeedChange As Single

    ''' <summary>
    ''' Velocidade do cursor quando ele se desloca de um componente para outro.
    ''' </summary>
    ''' <returns> Velocidade </returns>
    Public Property SpeedCursor As Single

    ''' <summary>
    ''' Grava a cor do cursor para pintar.
    ''' </summary>
    Public WriteOnly Property CursorColorCurrent As Color
        Set(value As Color)
            Dim aux As Frame = CursorArrow
            aux.tint = value
            CursorArrow = aux

            aux = CursorClove
            aux.tint = value

            CursorClove = aux
        End Set
    End Property

    ''' <summary>
    ''' Frame de cursor Arrow
    ''' </summary>
    ''' <returns> Frame de cursor </returns>
    Public Property CursorArrow As Frame
        Get
            Return _CursorArrow
        End Get
        Set(value As Frame)
            _CursorArrow = value
        End Set
    End Property

    ''' <summary>
    ''' Frame de cursor Clove
    ''' </summary>
    ''' <returns> Frame de cursor </returns>
    Public Property CursorClove As Frame
        Get
            Return _CursorClove
        End Get
        Set(value As Frame)
            _CursorClove = value
        End Set
    End Property

    Public ReadOnly Property Area As Vector2
        Get
            Return _area
        End Get
    End Property

    Public ReadOnly Property CursorPosition As Vector2
        Get
            Return _CursorPosition
        End Get
    End Property

    Public Property CursorMovementAxisMode As AxisMode

    ''' <summary>
    ''' Linha horizontal do cursor em cruz.
    ''' </summary>
    Private _HorizontalLine As Frame

    ''' <summary>
    ''' Linha vertical do cursor em cruz
    ''' </summary>
    Private _VerticalLine As Frame


    Private _CursorArrow As Frame
    Private _CursorClove As Frame
    Private _CursorColor As Color
    Private _CursorMode As CursorMode
    Private _CursorPosition As Vector2
    Private _Current As Integer
    Private _area As Vector2
    Private changePosition As Vector2

    ''' <summary>
    ''' Pilha de objetos focados.
    ''' </summary>
    Private focusStack As Stack(Of GUIObject)

    ''' <summary>
    ''' Ultimo objeto focado.
    ''' </summary>
    Private lastFocused As GUIObject

    ''' <summary>
    ''' Cria novo contexto
    ''' </summary>
    ''' <param name="area"> Area do contexto </param>
    ''' <param name="horizontalLine"> Linha horizontal do cursor em cruz </param>
    ''' <param name="verticalLine"> Linha vertical do cursor em cruz </param>
    Public Sub New(ByVal area As Vector2, ByVal horizontalLine As Frame, ByVal verticalLine As Frame)
        Me.New(area, horizontalLine, verticalLine, Nothing, Nothing, Nothing)
    End Sub

    ''' <summary>
    ''' Cria novo contexto padrão com cursores Arrow e Clove.
    ''' </summary>
    ''' <param name="area"> Area do contexto </param>
    Public Sub New(ByVal area As Vector2)
        Me.New(area, Nothing, Nothing, New Frame(GUIController.Texture, New Rectangle(96, 0, 16, 16)), New Frame(GUIController.Texture, New Rectangle(112, 0, 16, 16)), Nothing)
    End Sub

    ''' <summary>
    ''' Cria novo contexto padrão com cursores fornecidos
    ''' </summary>
    ''' <param name="area"> Area do contexto </param>
    ''' <param name="horizontalLine"> Frame horizontal do cursor em cruz </param>
    ''' <param name="verticalLine"> Frame vertical do cursor em cruz </param>
    ''' <param name="cursorArrow"> Frame do Cursor Arrow </param>
    ''' <param name="cursorClove"> Frame do Cursor Clove </param>
    ''' <param name="selectFrame"> Frame de seleção </param>
    Public Sub New(ByVal area As Vector2, ByVal horizontalLine As Frame, ByVal verticalLine As Frame, ByVal cursorArrow As Frame, ByVal cursorClove As Frame, ByVal selectFrame As Frame)
        MyBase.New(0, 0, 0, Vector2.Zero)
        Me.Controllers = New Dictionary(Of Integer, GUIObject)
        Me._HorizontalLine = horizontalLine
        Me._VerticalLine = verticalLine
        Me.DrawEnable = True
        Me.UpdateEnable = True
        _HorizontalLine.infinity = True
        _VerticalLine.infinity = True
        _area = area
        Me.CursorEnable = True
        Me.MovableCursor = True
        Me.Scale = Vector2.One
        Me.GUIObjectEnable = True
        Me.CursorColorCurrent = Color.White
        Me.CursorMode = CursorMode.Arrow
        Me.CursorArrow = cursorArrow
        Me.CursorClove = cursorClove
        Me.SpeedChange = 4
        Me.SpeedCursor = 16
        Me.UpdateEnable = False
        Me.CursorMovementAxisMode = AxisMode.Click

        If selectFrame.texture IsNot Nothing Then
            SelectBox = New Box(area, selectFrame)
        Else
            SelectBox = Nothing
        End If

        focusStack = New Stack(Of GUIObject)
        lastFocused = Me

        AxisModeSetting(Axis.Fire0) = AxisMode.Click
        AxisModeSetting(Axis.Fire1) = AxisMode.Click
        AxisModeSetting(Axis.Fire2) = AxisMode.Click
        AxisModeSetting(Axis.Horizontal) = AxisMode.Click
        AxisModeSetting(Axis.Vertical) = AxisMode.Click
        AxisModeSetting(Axis.Cancel) = AxisMode.Click
        AxisModeSetting(Axis.Submit) = AxisMode.Click
        OnFire0 = AddressOf SubmitFire0Function
        OnSubmit = AddressOf SubmitFire0Function
        OnCancel = AddressOf CancelFunction
    End Sub

    ''' <summary>
    ''' Desenha o GUIContext e seus componentes.
    ''' </summary>
    ''' <param name="spriteBatch"> SpriteBatch </param>
    ''' <param name="positionDelta"> Posição adicional </param>
    ''' <param name="scaleDelta"> Escala adicional </param>
    ''' <param name="angleDelta"> Angulo adicional </param>
    ''' <param name="layerDepthDelta"> Camada de profundidade de sprite adicional </param>
    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        If CursorEnable AndAlso Focus Then
            ' Cursor ativado e focado, desenha o cursor
            Dim posScreen As Vector2 = PositionTranslated - _area / 2 + New Vector2(_HorizontalLine.origin.X, _VerticalLine.origin.Y) + positionDelta

            Dim scaleX As Single = _area.X / _HorizontalLine.source.Width
            Dim scaleY As Single = _area.Y / _VerticalLine.source.Height

            _HorizontalLine.Draw(spriteBatch, New Vector2(posScreen.X, _CursorPosition.Y) - _HorizontalLine.origin, New Vector2(scaleX, 1), 0, LayerDetph + layerDepthDelta + 10)
            _VerticalLine.Draw(spriteBatch, New Vector2(_CursorPosition.X, posScreen.Y) - _VerticalLine.origin, New Vector2(1, scaleY), 0, LayerDetph + layerDepthDelta + 10)

            If CursorMode = CursorMode.Arrow Then
                CursorArrow.Draw(spriteBatch, New Vector2(_CursorPosition.X, _CursorPosition.Y), Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta + 11)
            ElseIf CursorMode = CursorMode.Clove Then
                CursorClove.Draw(spriteBatch, New Vector2(_CursorPosition.X, _CursorPosition.Y), Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta + 11)
            End If
        End If

        If (Selected OrElse Focus) AndAlso SelectBox IsNot Nothing Then
            SelectBox.Draw(spriteBatch, positionDelta + PositionTranslated, scaleDelta * Scale, angleDelta + Angle, layerDepthDelta + LayerDetph)
        End If

        For Each guiObj As GUIObject In Controllers.Values
            If guiObj IsNot Nothing AndAlso guiObj.DrawEnable Then
                guiObj.Draw(spriteBatch, PositionTranslated + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Adiciona GUIObject como filho
    ''' </summary>
    ''' <param name="obj"></param>
    Public Sub Add(ByRef obj As GUIObject)
        Controllers(obj.Index) = obj
        obj.Parent = Me
    End Sub

    ''' <summary>
    ''' Remove GUIObject
    ''' </summary>
    ''' <param name="obj"></param>
    Public Sub Remove(ByRef obj As GUIObject)
        If Controllers(obj.Index) Is obj Then
            Controllers(obj.Index) = Nothing
        End If
    End Sub

    ''' <summary>
    ''' Pega GUIObject selecionado
    ''' </summary>
    ''' <returns></returns>
    Public Function GetCurrent() As GUIObject
        Try
            Return Controllers(_Current)
        Catch ex As Exception
            For Each c In Controllers
                If c.Value IsNot Nothing Then
                    _Current = c.Key
                    Return Controllers(_Current)
                End If
            Next
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Pega objeto das cordanadas de índice X e Y.
    ''' </summary>
    ''' <param name="x"> Índice X </param>
    ''' <param name="y"> Índice Y </param>
    ''' <returns></returns>
    Private Function GetObjectOfCoordinates(ByVal x As Integer, ByVal y As Integer) As GUIObject
        For Each obj As GUIObject In Controllers.Values
            If obj.IndexX = x And obj.IndexY = y Then
                Return obj
            End If
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Pega maior Índice X
    ''' </summary>
    ''' <returns></returns>
    Private Function GetMaxX() As Integer
        Dim output As Integer = 0
        For Each obj As GUIObject In Controllers.Values
            If obj.IndexX > output Then
                output = obj.IndexX
            End If
        Next
        Return output
    End Function

    ''' <summary>
    ''' Pega maior Índice Y
    ''' </summary>
    ''' <returns></returns>
    Private Function GetMaxY() As Integer
        Dim output As Integer = 0
        For Each obj As GUIObject In Controllers.Values
            If obj.IndexY > output Then
                output = obj.IndexY
            End If
        Next
        Return output
    End Function

    ''' <summary>
    ''' Troca seleção para direção ativada
    ''' </summary>
    ''' <param name="up"> Flag de direção UP </param>
    ''' <param name="down"> Flag de direção DOWN </param>
    ''' <param name="left"> Flag de direção LEFT </param>
    ''' <param name="right"> Flag de direção RIGHT </param>
    Public Sub Move(ByVal up As Boolean, ByVal down As Boolean, ByVal left As Boolean, ByVal right As Boolean)
        Dim current As GUIObject = GetCurrent()
        Dim maxX As Integer = GetMaxX()
        Dim maxY As Integer = GetMaxY()

        Dim x As Integer = current.IndexX - right + left
        Dim y As Integer = current.IndexY + down - up
        current.Selected = False
        current.SelectedChange()

        current = Nothing
        Dim countSearchX As UInteger = 0
        Dim countSearchY As UInteger = 0
        While (current Is Nothing OrElse Not current.GUIObjectEnable)
            If countSearchX > 1 Then
                countSearchX = 0
                countSearchY += 1
                y += 1
            End If

            If countSearchY > 1 Then
                countSearchY = 0
                countSearchX += 1
                x += 1
            End If

            If x > maxX Then
                x = 0
                countSearchX += 1
            ElseIf x < 0 Then
                x = maxX
                countSearchX += 1
            End If

            If y < 0 Then
                y = maxY
                countSearchY += 1
            ElseIf y > maxY Then
                y = 0
                countSearchY += 1
            End If

            current = GetObjectOfCoordinates(x, y)
            If right Then
                x += 1
            ElseIf left Then
                x -= 1
            ElseIf down Then
                y -= 1
            ElseIf up Then
                y += 1
            End If
        End While

        current.Selected = True
        current.SelectedChange()

        CursorMode = current.CursorMode
        If current.CursorColor IsNot Nothing Then
            CursorColorCurrent = current.CursorColor.Invoke(current)
        Else
            CursorColorCurrent = Color.White
        End If

        _Current = current.Index
    End Sub

    ''' <summary>
    ''' Chamado no primeiro loop do GUIContext, e substitui o Update nesse loop, após ser setado como o GUIContext atual do GUIController.
    ''' (Evita que comandos de Input do GUIContext anterior se espalhe para o novo. )
    ''' </summary>
    Public Sub Load()
        UpdateEnable = True
        Dim current As GUIObject = GetCurrent()
        If current IsNot Nothing Then
            current.Selected = True
            current.SelectedChange()
            UpdateCursor(current)
        End If
    End Sub

    Public Sub SelectObject(x As Integer, y As Integer)
        Dim newCurrent As GUIObject = GetObjectOfCoordinates(x, y)

        If newCurrent IsNot Nothing Then
            Dim current As GUIObject = GetCurrent()

            current.Selected = False
            current.SelectedChange()

            newCurrent.Selected = True
            newCurrent.SelectedChange()

            CursorMode = newCurrent.CursorMode
            If newcurrent.CursorColor IsNot Nothing Then
                CursorColorCurrent = newCurrent.CursorColor.Invoke(newCurrent)
            Else
                CursorColorCurrent = Color.White
            End If
            _Current = newCurrent.Index
        End If
    End Sub

    ''' <summary>
    ''' Update do GUIContext
    ''' Chamado a cada loop do jogo, quando setado como GUIContext atual.
    ''' </summary>
    ''' <param name="gameTime"> Tempo de jogo </param>
    Public Overrides Sub Update(gameTime As GameTime)
        If Focus Then
            ' Se focado atualiza.
            ' Evita que comandos de Input que o destino é o controle focado seja espalhado para o GUIContext atual.
            Dim elapse As Single = gameTime.ElapsedGameTime.TotalSeconds

            ' Componente selecionado atual
            Dim current As GUIObject = GetCurrent()

            Dim updatesCursor As Boolean = False
            updatesCursor = InvokeAxisFulled(gameTime) OrElse updatesCursor

            If CursorEnable AndAlso current IsNot Nothing Then
                ' Translada pouco a pouco a posição do cursor até o destino.
                _CursorPosition += ((current.PositionTranslated + current.Origin * current.Scale - _CursorPosition)) * gameTime.ElapsedGameTime.TotalSeconds * SpeedCursor

                If MovableCursor Then
                    If CursorMovementAxisMode = AxisMode.Floating Then
                        changePosition += New Vector2(Input.ReadAxis(Axis.Horizontal), Input.ReadAxis(Axis.Vertical)) * elapse * SpeedChange
                    Else
                        changePosition -= New Vector2(Input.ReadAxisClick(Axis.Horizontal), Input.ReadAxisClick(Axis.Vertical))
                    End If

                    If changePosition.X >= 1 Then
                        'Right
                        Move(False, False, False, True)
                        changePosition.X = 0
                    ElseIf changePosition.X <= -1 Then
                        'Left
                        Move(False, False, True, False)
                        changePosition.X = 0
                    ElseIf changePosition.Y >= 1 Then
                        'Up
                        Move(True, False, False, False)
                        changePosition.Y = 0
                    ElseIf changePosition.Y <= -1 Then
                        'Down
                        Move(False, True, False, False)
                        changePosition.Y = 0
                    End If
                End If

                updatesCursor = current.InvokeAxisFulled(gameTime) OrElse updatesCursor

                If updatesCursor Then
                    UpdateCursor(current)
                End If
            End If

        End If

        ' Pecorre uma nova lista com os controles (evita erro de alteração)
        For Each element As GUIObject In New List(Of GUIObject)(Controllers.Values)
            If element IsNot Nothing AndAlso element.UpdateEnable Then
                element.Update(gameTime)
            End If
        Next

    End Sub

    ''' <summary>
    ''' Atualiza cursor de acordo com configurações do objeto selecionado atual
    ''' </summary>
    ''' <param name="current"> Objeto selecionado </param>
    Private Sub UpdateCursor(ByVal current As GUIObject)
        CursorMode = current.CursorMode
        If current.CursorColor IsNot Nothing Then
            CursorColorCurrent = current.CursorColor.Invoke(current)
        Else
            CursorColorCurrent = Color.White
        End If
    End Sub

    ''' <summary>
    ''' Função chamada ao precionar Axis.Submit ou Axis.Fire0.
    ''' Simplemente foca o objeto selecionado atual no contexto.
    ''' </summary>
    ''' <param name="context"> Contexto pai do objeto que disparou o Input </param>
    ''' <param name="obj"> Objeto que disparou o Input </param>
    ''' <param name="axisValue"> Valor do axis que disparou o Input </param>
    ''' <param name="axis"> Axis que disparou o Input </param>
    Protected Sub SubmitFire0Function(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        Me.FocusOn(Me.GetCurrent())
    End Sub

    ''' <summary>
    ''' Foca em um objeto de GUI
    ''' </summary>
    ''' <param name="obj"> Objeto para focar </param>
    Public Sub FocusOn(ByRef obj As GUIObject)
        If obj IsNot lastFocused AndAlso obj IsNot Nothing Then
            focusStack.Push(lastFocused)
            lastFocused.Focus = False
            lastFocused = obj
            If TypeOf lastFocused Is GUIContext Then
                Dim lastFocusedContext As GUIContext = TryCast(lastFocused, GUIContext)
                ScreenManager.Instance.Current.GUIController.ChangeContext(lastFocusedContext)
            End If
            obj.Focus = True
        End If
    End Sub

    ''' <summary>
    ''' Desfoca do objeto atualmente focado e volta a focar o objeto anteriormente focado.
    ''' </summary>
    Public Sub Refocus()
        If TypeOf lastFocused Is GUIContext Then
            Dim lastFocusedContext As GUIContext = TryCast(lastFocused, GUIContext)
            ScreenManager.Instance.Current.GUIController.GoBack()
        ElseIf focusStack.Count > 0 Then
            lastFocused.Focus = False
            lastFocused = focusStack.Pop()
            lastFocused.Focus = True
        End If
    End Sub

    ''' <summary>
    ''' Reseta a pilha de foco e desfoca o ultimo focado, retornando a focar o proprio GUIContext.
    ''' </summary>
    Public Sub ResetFocus()
        focusStack.Clear()
        lastFocused.Focus = False
        lastFocused = Me
    End Sub

    ''' <summary>
    ''' Chamado qunado focado
    ''' </summary>
    Public Overrides Sub Focused()

    End Sub

    ''' <summary>
    ''' Chamado quando selecionado
    ''' </summary>
    Public Overrides Sub SelectedChange()

    End Sub

    ''' <summary>
    ''' Retorna um valor negativo diferente sempre que chamado.
    ''' (Contador reseta quando é limpo com Clear())
    ''' </summary>
    ''' <returns></returns>
    Public Function NextNegative() As Integer
        negativeCounter -= 1
        Return negativeCounter
    End Function

    ''' <summary>
    ''' Limpa todos os controles e reseta contador negativo.
    ''' </summary>
    Public Sub Clear()
        Controllers.Clear()
        negativeCounter = 0
    End Sub
End Class
