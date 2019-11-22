Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics
Imports NavalBattle

''' <summary>
''' Objeto grafico do mapa
''' </summary>
Public Class NavalMap
    Inherits GUIObject

    Private mapEdge As Box
    Private tilemap As Tilemap(Of ShipTilemap)
    Private tilemapStatus As Tilemap(Of HouseStatus)
    Private water As Tilemap(Of Integer)

    Private sizeX As Integer
    Private sizeY As Integer

    Private naval As Texture2D

    Private area As Vector2
    Private insideArea As Vector2
    Private scaleMap As Single

    Private updates As IList(Of IUpdate)
    Private drawings As IList(Of IDrawable)
    Private fire0 As GUIObject.OnAxis
    Private fire1 As GUIObject.OnAxis
    Private fire2 As GUIObject.OnAxis
    Private calculateColor As CalculateColor

    Public Sub New(context As GUIContext, naval As Texture2D, area As Vector2, sizeX As Integer, sizeY As Integer, fire0 As GUIObject.OnAxis, fire1 As GUIObject.OnAxis, fire2 As GUIObject.OnAxis, calculateColor As CalculateColor)
        MyBase.New(-1, -1, -1, Vector2.Zero)
        Me.DrawEnable = True
        Me.UpdateEnable = True
        Me.GUIObjectEnable = False
        Me.Scale = Vector2.One
        Me.naval = naval
        Me.sizeX = sizeX
        Me.sizeY = sizeY
        Me.Parent = context
        Me.fire0 = fire0
        Me.fire1 = fire1
        Me.fire2 = fire2
        Me.calculateColor = calculateColor
        updates = New List(Of IUpdate)
        drawings = New List(Of IDrawable)

        Me.area = area

        scaleMap = CalculateScale()
        insideArea = CalculateInsideArea()

        mapEdge = New Box(insideArea + New Vector2(16, 16), New Frame(naval, New Rectangle(64, 96, 24, 24)))
        tilemap = CreateTilemap()
        tilemapStatus = CreateTilemapStatus()
        water = CreateWater()
        CreateGUIContext(context)

        updates.Add(tilemap)
        updates.Add(water)

        drawings.Add(mapEdge)
        drawings.Add(water)
        drawings.Add(tilemap)
        drawings.Add(tilemapStatus)

        context.Add(Me)


        mapEdge.LayerDepth = 1
        water.LayerDepth = 2
        tilemap.LayerDepth = 3
        tilemapStatus.LayerDepth = 5
        context.LayerDetph = 6
    End Sub

    Public Overrides Sub Update(gameTime As GameTime)
        For Each element As IUpdate In updates
            If element.UpdateEnable Then
                element.Update(gameTime)
            End If
        Next
    End Sub

    Public Sub SetHouse(x As Integer, y As Integer, ship As Ship, orientation As Orientation, piece As Integer)
        Dim shipTile As ShipTilemap = ShipTilemap.None
        If orientation = Orientation.Horizontal Then
            Select Case ship
                Case Ship.None
                    shipTile = ShipTilemap.None
                Case Ship.Submarine
                    shipTile = ShipTilemap.Submarine
                Case Ship.Destroyer
                    shipTile = ShipTilemap.Destroyer0
                Case Ship.Battleship
                    shipTile = ShipTilemap.Battleship0
                Case Ship.Carrier
                    shipTile = ShipTilemap.Carrier0
            End Select
        ElseIf orientation = Orientation.Vertical Then
            Select Case ship
                Case Ship.None
                    shipTile = ShipTilemap.None
                Case Ship.Submarine
                    shipTile = ShipTilemap.VSubmarine
                Case Ship.Destroyer
                    shipTile = ShipTilemap.VDestroyer0
                Case Ship.Battleship
                    shipTile = ShipTilemap.VBattleship0
                Case Ship.Carrier
                    shipTile = ShipTilemap.VCarrier0
            End Select
        End If
        If ship <> Ship.None Then
            shipTile = shipTile + piece
        End If
        tilemap.SetIndex(x, y, shipTile)
    End Sub

    Public Sub SetHouse(x As Integer, y As Integer, houseStatus As HouseStatus)
        tilemapStatus.SetIndex(x, y, houseStatus)
    End Sub

    Private Function CalculateInsideArea() As Vector2
        Dim insideArea As Vector2

        If (area.X / sizeX > area.Y / sizeY) Then
            insideArea = New Vector2(sizeX * scaleMap * 16.0F, area.Y - 16.0F)
        Else
            insideArea = New Vector2(area.X - 16.0F, sizeY * scaleMap * 16.0F)
        End If

        Return insideArea
    End Function

    Private Function CalculateScale() As Single
        Dim scale As Single
        If sizeX >= sizeY Then
            scale = (area.X - 16.0F) / (sizeX * 16.0F)
        Else
            scale = (area.Y - 16.0F) / (sizeY * 16.0F)
        End If
        Return scale
    End Function

    Private Shared Sub ButtonClick(context As GUIContext, obj As GUIObject, axisValue As Single, axis As Axis)
        If TypeOf obj Is Button Then
            Dim button As Button = TryCast(obj, Button)
            Select Case axis
                Case Axis.Fire0
                    button.OnFire0 = Nothing
                Case Axis.Fire1
                    button.OnFire1 = Nothing
                Case Axis.Fire2
                    button.OnFire2 = Nothing
            End Select

            If button.OnFire0 Is Nothing AndAlso button.OnFire1 Is Nothing AndAlso button.OnFire2 Is Nothing Then
                button.CursorMode = CursorMode.Arrow
            End If
        End If
    End Sub

    Private Sub CreateGUIContext(context As GUIContext)

        Dim color As Color = New Color(200, 200, 200, 200)
        Dim horizontalFrame As Frame = New Frame(naval, New Vector2(4, 4), New Rectangle(80, 120, 8, 8), color, SpriteEffects.None)
        Dim verticalFrame As Frame = New Frame(naval, New Vector2(4, 4), New Rectangle(88, 120, 8, 8), color, SpriteEffects.None)

        Dim selectFrame As Frame = New Frame(naval, New Rectangle(120, 88, 6, 6))

        Dim scale As Vector2 = New Vector2(insideArea.X / (16.0F * sizeX), insideArea.Y / (16.0F * sizeY))
        Dim interval As Vector2 = New Vector2(insideArea.X / sizeX, insideArea.Y / sizeY)
        Dim position As Vector2 = New Vector2(insideArea.X / 2.0F, insideArea.Y / 2.0F)

        For i As Integer = 0 To sizeX - 1
            For j As Integer = 0 To sizeY - 1
                Dim button As Button = New Button(i + j * sizeX, i, j, New Vector2(i * interval.X, j * interval.Y) - position, selectFrame, New Vector2(16, 16))
                button.LayerDetph = 3
                button.Scale = scale
                button.Origin = Vector2.One * 8.0F
                context.Add(button)
                button.Parent = Me
                button.CursorColor = calculateColor
                button.OnFire0 = Me.fire0
                button.OnFire1 = Me.fire1
                button.OnFire2 = Me.fire2
                button.OnCancel = AddressOf ButtonClick
            Next
        Next
    End Sub

    Private Function CreateWater() As Tilemap(Of Integer)
        Dim tilemap As Tilemap(Of Integer) = New Tilemap(Of Integer)(sizeX, sizeY)
        tilemap.Scale = New Vector2(insideArea.X / (16.0F * sizeX), insideArea.Y / (16.0F * sizeY))


        Dim water As AnimatedSprite(Of Integer) = New AnimatedSprite(Of Integer)(Vector2.Zero)
        Dim listFrames As IList(Of Frame) = New List(Of Frame)
        listFrames.Add(New Frame(naval, New Rectangle(0, 0, 16, 16)))
        listFrames.Add(New Frame(naval, New Rectangle(128, 0, 16, 16)))
        Dim animationWater As Animation = New Animation(listFrames)
        water.UpdatesPerSecond = 1
        water.Animations.Add(0, animationWater)
        water.animation = 0
        water.SetFrameIndex(0)

        tilemap.Sprites.Add(0, water)

        Return tilemap
    End Function

    Private Function CreateTilemapStatus() As Tilemap(Of HouseStatus)
        Dim tilemap As Tilemap(Of HouseStatus) = New Tilemap(Of HouseStatus)(sizeX, sizeY)
        tilemap.Scale = New Vector2(insideArea.X / (16.0F * sizeX), insideArea.Y / (16.0F * sizeY))

        tilemap.Sprites.Add(HouseStatus.Normal, CreateTileSprite(112, 0))
        tilemap.Sprites.Add(HouseStatus.Hit, CreateTileSprite(0, 48))
        tilemap.Sprites.Add(HouseStatus.Missed, CreateTileSprite(16, 48))

        Return tilemap
    End Function

    Private Function CreateTilemap() As Tilemap(Of ShipTilemap)
        Dim tilemap As Tilemap(Of ShipTilemap) = New Tilemap(Of ShipTilemap)(sizeX, sizeY)

        tilemap.Scale = New Vector2(insideArea.X / (16.0F * sizeX), insideArea.Y / (16.0F * sizeY))

        tilemap.Sprites.Add(ShipTilemap.None, CreateTileSprite(112, 0))

        tilemap.Sprites.Add(ShipTilemap.Submarine, CreateTileSprite(64, 32))

        tilemap.Sprites.Add(ShipTilemap.Destroyer0, CreateTileSprite(32, 16))
        tilemap.Sprites.Add(ShipTilemap.Destroyer1, CreateTileSprite(48, 16))
        tilemap.Sprites.Add(ShipTilemap.Destroyer2, CreateTileSprite(64, 16))

        tilemap.Sprites.Add(ShipTilemap.Battleship0, CreateTileSprite(0, 32))
        tilemap.Sprites.Add(ShipTilemap.Battleship1, CreateTileSprite(16, 32))
        tilemap.Sprites.Add(ShipTilemap.Battleship2, CreateTileSprite(32, 32))
        tilemap.Sprites.Add(ShipTilemap.Battleship3, CreateTileSprite(48, 32))

        tilemap.Sprites.Add(ShipTilemap.Carrier0, CreateTileSprite(16, 0))
        tilemap.Sprites.Add(ShipTilemap.Carrier1, CreateTileSprite(32, 0))
        tilemap.Sprites.Add(ShipTilemap.Carrier2, CreateTileSprite(32, 0))
        tilemap.Sprites.Add(ShipTilemap.Carrier3, CreateTileSprite(48, 0))
        tilemap.Sprites.Add(ShipTilemap.Carrier4, CreateTileSprite(32, 0))
        tilemap.Sprites.Add(ShipTilemap.Carrier5, CreateTileSprite(64, 0))


        tilemap.Sprites.Add(ShipTilemap.VSubmarine, CreateTileSpriteV(64, 32))
        tilemap.Sprites.Add(ShipTilemap.VDestroyer0, CreateTileSpriteV(32, 16))
        tilemap.Sprites.Add(ShipTilemap.VDestroyer1, CreateTileSpriteV(48, 16))
        tilemap.Sprites.Add(ShipTilemap.VDestroyer2, CreateTileSpriteV(64, 16))

        tilemap.Sprites.Add(ShipTilemap.VBattleship0, CreateTileSpriteV(0, 32))
        tilemap.Sprites.Add(ShipTilemap.VBattleship1, CreateTileSpriteV(16, 32))
        tilemap.Sprites.Add(ShipTilemap.VBattleship2, CreateTileSpriteV(32, 32))
        tilemap.Sprites.Add(ShipTilemap.VBattleship3, CreateTileSpriteV(48, 32))

        tilemap.Sprites.Add(ShipTilemap.VCarrier0, CreateTileSpriteV(16, 0))
        tilemap.Sprites.Add(ShipTilemap.VCarrier1, CreateTileSpriteV(32, 0))
        tilemap.Sprites.Add(ShipTilemap.VCarrier2, CreateTileSpriteV(32, 0))
        tilemap.Sprites.Add(ShipTilemap.VCarrier3, CreateTileSpriteV(48, 0))
        tilemap.Sprites.Add(ShipTilemap.VCarrier4, CreateTileSpriteV(32, 0))
        tilemap.Sprites.Add(ShipTilemap.VCarrier5, CreateTileSpriteV(64, 0))

        Return tilemap
    End Function

    Private Function CreateTileSprite(x As Integer, y As Integer) As Sprite
        Dim sprite As Sprite = New Sprite()
        sprite.Frame = New Frame(naval, New Rectangle(x, y, 16, 16))
        Return sprite
    End Function

    Private Function CreateTileSpriteV(x As Integer, y As Integer) As Sprite
        Dim sprite As Sprite = CreateTileSprite(x, y)
        Dim frame As Frame = sprite.Frame

        frame.origin = New Vector2(0, 16)

        sprite.Frame = frame
        sprite.Angle = 90
        Return sprite
    End Function

    Public Overrides Sub Draw(ByRef spriteBatch As SpriteBatch, positionDelta As Vector2, scaleDelta As Vector2, angleDelta As Single, layerDepthDelta As UShort)
        For Each element As IDrawable In drawings
            If element.DrawEnable Then
                element.Draw(spriteBatch, PositionTranslated + positionDelta, Scale * scaleDelta, Angle + angleDelta, LayerDetph + layerDepthDelta)
            End If
        Next
    End Sub
End Class
