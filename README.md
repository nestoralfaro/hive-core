# First iteration of our Game Core for Hive
Todo:
- Unit tests.
- Implement GetAntMovingPositions
- Implement GetBeetleMovingPositions
- Implement GetGrasshopperMovingPositions
- Implement GetQueenBeeMovingPositions
- Implement GetSpiderMovingPositions
- Managing turns and keeping track of pieces.
- Validating movements and enforcing the rules of the game–e.g., Fix Regex tha validates inputs
- Refactor–e.g., remove unnecessary variables, data types, etc. Just improve the overall implementation not only in speed, but also in readability.
- Make sure that the player moves only THEIR pieces and not their opponent's.

If you have .NET on your machine, just run it with:
```
dotnet run
```

## Based on [Wastack's README](https://github.com/Wastack/hive_AI/blob/master/README.md)
 - __Piece__:
    A piece is represented by 3 chars.
    * The first is the color and can be "black" or "white" {`b`|`w`}.
    * The second is the piece name {`A`|`B`|`G`|`Q`|`S`}.
    * The third is the piece number used to distinguish pieces of the same
        name {`1`|`2`|`3`}.
    * ex: `wG2` - represents the second played grasshopper played by the
        whites.

 - __Move__:
    A move can be a placement of a piece from the player set into the board or
    moving a piece from its current position to a new position. Both of this
    actions are represented in the same way.
    A move representation has 3 sections:
        (action piece)+(point of contact)+(target piece)
    * The first section is the notation of the action piece.
    * The second is the point of contact with the piece where the move stops.
    * The third is the notation of the piece refered by the contact point.
    * ex: `wG2*/bA1` - the second white grasshopper is positioned touching the
        first black ant on its left-upper side.

 - __Point Of Contact__:
    To represent how the pieces are placed next to each other we refer to wich
    face of the target piece the action piece will be touching by the end of
    the action.
    * `*/` - NW (north-west) active piece is place touching the target piece
        at its upper-left face.
    * `*|` - W (west) active piece is place touching the target piece at its
        left face.
    * `*\` - SW (south-west) active piece is place touching the target piece
        at its lower-left face.
    * `\*` - NE (noth-east) active piece is place touching the target piece at
        its upper-right face.
    * `|*` - E (east) active piece is place touching the target piece at its
        right face.
    * `/*` - SE (south-east) active piece is place touching the target piece
        at its lower-right face.
    * `=*` - O (over) active piece is place on top of the target piece.

 - __Starting Piece__:
    The only exception to the previous notation is the starting piece of the
    first player. The first move of the first player is just the name of a
    piece since the placement of the 1st piece is always fixed.

 - __Side OffSets__:
    The only exception to the previous notation is the starting piece of the
    first player. The first move of the first player is just the name of a
    piece since the placement of the 1st piece is always fixed.
    ```
        {
            // Notice how each side is only valid if it adds up to an even number
            { "*/", (-1, 1) },   // [0] Northwest
            { "*|", (-2, 0) },   // [1] West
            { "*\\", (-1, -1) }, // [2] Southwest
            { "/*", (1, -1) },   // [3] Southeast
            { "|*", (2, 0) },    // [4] East
            { "\\*", (1, 1) },   // [5] Northeast
        }
    ```

    Another interesting property of this approach is that for any `point` if `(point % side == (0, 0))`, then it is a valid side of the piece. If necessary, you may consider a validation as
    ```
        bool IsNeighbour = (point % side == (0, 0));
    ``` 

## Unit Tests with xUnit
- Make sure you review [this video](https://www.youtube.com/watch?v=HQmbAdjuB88&t=495s) to install the right packages and test it on VS Code.
- See its [assertions](https://textbooks.cs.ksu.edu/cis400/1-object-orientation/04-testing/05-xunit-assertions/)
- Write your tests in `Tests.cs`.
- To run
```
dotnet test
```