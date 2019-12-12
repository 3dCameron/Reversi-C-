namespace Reversi {
    class Cell
    {
        // An array containing the eight surrounding cells. Initialized by the board. Start with East and move counter-clockwise.
        // Index key: 0 = E, 1 = NE, 2 = N, 3 = NW, 4 = W, 5 = SW, 6 = S, 7 = SE
        Cell[] neighbors;

        // Internally, we track state as a byte in order to save memory.
        // 0 = Empty, 1 = Black, and 2 = White
        byte state = 0;


        ////////////////
        // CONVERSION //
        ////////////////

        private static byte GetStateAsByte(char state) {
            if(state == 'B') {
                return 1;
            } else if (state == 'W') {
                return 2;
            } else {
                return 0;
            }
        }


        ///////////////
        // ACCESSORS //
        ///////////////

        // Boolean method for efficiently checking whether a cell contains a piece or not.
        public bool IsEmpty() {
            return (state == 0);
        }

        // Get state in byte form.
        public byte GetState() {
            return state;
        }

        // Get state by its character representation for easy representation on the board.
        public char GetStateAsChar() {
            if (state == 1) {
                return 'B';
            } else if (state == 2) {
                return 'W';
            } else {
                return '_';
            }
        }

        // Get the neighboring cell in the specified direction.
        public Cell GetNeighborAtDirection(byte direction) {
            if (neighbors != null) {
                return neighbors[direction];
            } else {
                return null;
            }
        }


        //////////////
        // MUTATORS //
        //////////////

        // Set the state to the provided state converted to byte form.
        public void SetState(char state) {
            this.state = GetStateAsByte(state);
        }

        public void SetNeighbors(Cell[] neighbors) {
            this.neighbors = neighbors;
        }


        //////////////
        // GAMEPLAY //
        //////////////

        // Attempts to place a piece, returning true if successful or false if not. 0 = B, 1 = W
        public bool PlacePiece(bool player) {
            bool success = false;

            byte playerState = (byte) (player ? 2 : 1);
            byte enemyState = (byte) (player ? 1 : 2);

            if (IsEmpty()) {
                for (byte direction = 0; direction < 8; direction++) {
                    if (neighbors[direction] != null) {
                        if (neighbors[direction].GetState() == enemyState) {
                            success = CascadeInDirection(direction, playerState, enemyState) | success; 
                        }
                    }
                }
            }

            return success;
        }

        // Recursively attempt to make all pieces in a direction the specified color, returning true if successful or false if not.
        private bool CascadeInDirection(byte direction, byte playerState, byte enemyState) {
            Cell nextPiece = neighbors[direction];

            if (nextPiece != null) {
                // RECURSIVE CASE
                if (nextPiece.GetState() == enemyState) {
                    // This could still be a valid move.
                    if (nextPiece.CascadeInDirection(direction, playerState, enemyState)) {
                        // We've confirmed that the final piece of the sequence is the color of the current player, so flip ourselves to match.
                        state = playerState;

                        // Cascade back
                        return true;
                    }
                // BASE CASE
                } else if (nextPiece.GetState() == playerState) {
                    // We've confirmed that the final piece of the sequence is the color of the current player, so flip ourselves to match.
                    state = playerState;

                    // Cascade back
                    return true;
                }
            }

            return false;
        }

        // Return true if any of the eight directions has a legal move, otherwise false.
        public bool CheckForLegalMoves(bool player) {
            if (!IsEmpty()) {
                return false;
            }

            byte playerState = (byte) (player ? 2 : 1);
            byte enemyState = (byte) (player ? 1 : 2);

            for (byte direction = 0; direction < 8; direction++) {
                if (CheckForLegalMovesInDirection(direction, playerState, enemyState)) {
                    return true;
                }
            }
            return false;
        }

        // Return true if this line of pieces begins with the enemy color and is terminated by the player color.
        private bool CheckForLegalMovesInDirection(byte direction, byte playerState, byte enemyState) {
            Cell nextPiece = neighbors[direction];

            // First piece must be the enemy's color.
            if (nextPiece != null && nextPiece.GetState() == enemyState) {
                nextPiece = nextPiece.GetNeighborAtDirection(direction);
                if (nextPiece == null) {
                    return false;
                } else {
                    while (nextPiece.GetState() == enemyState) {
                        nextPiece = nextPiece.GetNeighborAtDirection(direction);
                        if (nextPiece == null) {
                            return false;
                        }
                    }
                    return (nextPiece.GetState() == playerState);
                }
            }

            return false;
        }

    }
}