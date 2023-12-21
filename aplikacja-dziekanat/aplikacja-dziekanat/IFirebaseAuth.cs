﻿using db;
using System.Threading.Tasks;
namespace aplikacja_dziekanat
{
    public interface IFirebaseAuth
    {
        User CurrentUser { get; }
        string Token();

        Task<string> LoginWithEmailAndPassword(string email, string password);
        Task<string> RegisterWithEmailAndPassword(string email, string password, User newUser);
        Task VerifyBeforeUpdateEmail(string newEmail);
        Task ChangeUserEmail(string newEmail);
        void ChangeUserPassword(string newPassword);
        void Logout();
    }
}
