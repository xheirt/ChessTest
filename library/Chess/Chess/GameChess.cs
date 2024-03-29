﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class GameChess
    {
        public string fen { get; private set; }
        Board board;
        Moves moves;
        List<FigureMoving> allMoves;

        public GameChess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }

        GameChess (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            moves = new Moves(board);
        }



        public GameChess Move (string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return this;                                // проверка можно ли сделать ход за определенную сторону
            if (board.IsCheckAfterMove(fm))
                return this;
            Board nextBoard = board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt (int x, int y)
        {
            Square square = new Square(x, y);
            Figure f = board.GetFigureAt(square);
            return f == Figure.none ? '.' : (char)f;
        }

        void FindAllMoves()            // Поиск всех ходов
        {
            allMoves = new List<FigureMoving>();
            foreach (FigureOnSquare fs in board.YieldFigures())
                foreach (Square to in Square.YieldSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if (moves.CanMove(fm))
                        if(!board.IsCheckAfterMove (fm))
                        allMoves.Add(fm);
                }
        }

        public List<string> GetAllMoves()           // Возвращает все ходы в строчном формате
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving fm in allMoves)
                list.Add(fm.ToString());
            return list;
        }

        public bool isCheck()                   // проверка на то, есть ли шах
        {
            return board.IsCheck();        
        }                                    
    }
}
