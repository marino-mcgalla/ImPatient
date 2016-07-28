using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using PatientApp.Models;
using System.Collections.Generic;

namespace PatientApp.Data
{
    public class SampleData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var db = serviceProvider.GetService<ApplicationDbContext>();
            if (db.Users.Any())
            { return; }
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            //db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var nurseUser1 = await userManager.FindByNameAsync("nurse1@cc.com");
            if (nurseUser1 == null)
            {
                nurseUser1 = new ApplicationUser
                {
                    UserName = "nurse1@cc.com",
                    Email = "nurse1@cc.com",
                    FirstName = "Nurse Rosie",
                    LastName = "Whitney",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };
                await userManager.CreateAsync(nurseUser1, "Secret123!");
                await userManager.AddClaimAsync(nurseUser1, new Claim("IsNurse", "true"));
            }

            var nurseUser2 = await userManager.FindByNameAsync("nurse2@cc.com");
            if (nurseUser2 == null)
            {
                nurseUser2 = new ApplicationUser
                {
                    UserName = "nurse2@cc.com",
                    Email = "nurse2@cc.com",
                    FirstName = "Nurse Jess",
                    LastName = "Hunter",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };
                await userManager.CreateAsync(nurseUser2, "Secret123!");
                await userManager.AddClaimAsync(nurseUser2, new Claim("IsNurse", "true"));
            }
            var patientUser1 = await userManager.FindByNameAsync("patient1@cc.com");
            if (patientUser1 == null)
            {
                patientUser1 = new ApplicationUser
                {
                    Email = "patient1@cc.com",
                    UserName = "patient1@cc.com",
                    FirstName = "Jackie",
                    LastName = "Ross",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };
                await userManager.CreateAsync(patientUser1, "Secret123!");
                await userManager.AddClaimAsync(patientUser1, new Claim("IsPatient", "true"));
            }
            var patientUser2 = await userManager.FindByNameAsync("patient2@cc.com");
            if (patientUser2 == null)
            {
                patientUser2 = new ApplicationUser
                {
                    Email = "patient2@cc.com",
                    UserName = "patient2@cc.com",
                    FirstName = "Aaron",
                    LastName = "Mikluscak",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };
                await userManager.CreateAsync(patientUser2, "Secret123!");
                await userManager.AddClaimAsync(patientUser2, new Claim("IsPatient", "true"));
            }
            db.SaveChanges();

            var nurseUserId = db.Users.Where(u => u.LastName == nurseUser1.LastName).FirstOrDefault().Id;
            var patientUserId = db.Users.Where(u => u.LastName == patientUser1.LastName).FirstOrDefault().Id;
            var nurse2UserId = db.Users.Where(u => u.LastName == nurseUser2.LastName).FirstOrDefault().Id;
            var patient2UserId = db.Users.Where(u => u.LastName == patientUser2.LastName).FirstOrDefault().Id;

            //add Nurse nurse
            if (!db.Nurses.Any())
            {
                var nurse1 = new Nurse
                {
                    ApplicationUserId = nurseUserId,
                    Messages = new List<Message>()
                };

                var nurse2 = new Nurse
                {
                    ApplicationUserId = nurse2UserId,
                    Messages = new List<Message>()
                };

                db.Nurses.AddRange(nurse1, nurse2);
                db.SaveChanges();
            };




            //add Patient patient
            if (!db.Patients.Any())
            {
                var patient1 = new Patient
                {
                    ApplicationUserId = patientUserId,
                    RoomNumber = 1,
                    BedNumber = 1,
                    CheckInDate = new DateTime(2016, 7, 11, 12, 45, 15),
                    Notes = "Grumpy person, needs lots of patient",
                    Dependency = 2,
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };

                var patient2 = new Patient
                {
                    ApplicationUserId = patient2UserId,
                    RoomNumber = 1,
                    BedNumber = 2,
                    CheckInDate = new DateTime(2016, 7, 11, 12, 45, 15),
                    Notes = "Very independent, easy to work with",
                    Dependency = 3,
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 7, 11, 12, 45, 15)
                };
                db.Patients.AddRange(patient1, patient2);
                db.SaveChanges();

            }

            var hunter = db.Users.Where(u => u.LastName == "Hunter").FirstOrDefault().Id;
            var nurseWhitney = db.Nurses.Where(n => n.ApplicationUserId == hunter).FirstOrDefault();
            var mikluscak = db.Users.Where(u => u.LastName == "Mikluscak").FirstOrDefault().Id;
            var whitneyCreated = db.Users.Where(u => u.LastName == "Whitney").FirstOrDefault().Id;
            var nurse = db.Nurses.Where(n => n.ApplicationUserId == whitneyCreated).FirstOrDefault();
            var rossCreated = db.Users.Where(u => u.LastName == "Ross").FirstOrDefault().Id;


            var patientId1 = db.Patients.Where(p => p.ApplicationUserId == mikluscak).FirstOrDefault();

            var patientId2 = db.Patients.Where(p => p.ApplicationUserId == rossCreated).FirstOrDefault();
            if (!db.NursePatients.Any())
            {
                var nurseId1 = db.Nurses.Where(n => n.ApplicationUserId == hunter).FirstOrDefault().Id;
                //var patientId1 = db.Patients.Where(p => p.ApplicationUserId == mikluscak).FirstOrDefault();
                var nurseId2 = db.Nurses.Where(n => n.ApplicationUserId == whitneyCreated).FirstOrDefault().Id;
                //var patientId2 = db.Patients.Where(p => p.ApplicationUserId == rossCreated).FirstOrDefault();
                patientId1.NurseId = nurseId1;
                patientId2.NurseId = nurseId2;

                var nursePatients = new NursePatient[]
                {
                    new NursePatient
                    {
                        NurseId = nurseId1,
                        PatientId = patientId1.Id
                    },
                    new NursePatient
                    {
                        NurseId = nurseId2,
                        PatientId = patientId2.Id
                    }
                };
                db.NursePatients.AddRange(nursePatients);

                db.SaveChanges();
            }



            if (!db.Messages.Any())
            {
                var nurse1Messages = new List<Message>
                {
                    new Message {Patient = patientId1, MessageString = "Water", TimeRequested = new DateTime(2016, 7, 11, 12, 45, 15) },
                    new Message {Patient = patientId1, MessageString = "Food", TimeRequested = new DateTime(2016, 7, 11, 1, 36, 40) },
                    new Message {Patient = patientId1, MessageString = "Emergency", TimeRequested = new DateTime(2016, 7, 11, 2, 03, 55) }
                };

                var nurse2Messages = new List<Message>
                {
                    new Message {Patient = patientId2, MessageString = "Water", TimeRequested = new DateTime(2016, 7, 11, 10, 00, 00) },
                    new Message {Patient = patientId2, MessageString = "Water", TimeRequested = new DateTime(2016, 7, 11, 10, 15, 02) },
                    new Message {Patient = patientId2, MessageString = "Water", TimeRequested = new DateTime(2016, 7, 11, 08, 30, 04) }
                };
                nurseWhitney.Messages.AddRange(nurse1Messages);
                nurse.Messages.AddRange(nurse2Messages);
                db.SaveChanges();
            }


            // Ensure db
            db.Database.EnsureCreated();

            // Ensure Stephen (IsAdmin)
            var stephen = await userManager.FindByNameAsync("Stephen.Walther@CoderCamps.com");
            if (stephen == null)
            {
                // create user
                stephen = new ApplicationUser
                {
                    UserName = "Stephen.Walther@CoderCamps.com",
                    Email = "Stephen.Walther@CoderCamps.com",
                    FirstName = "Stephen",
                    LastName = "Walther",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 6, 28, 12, 45, 15)
                };
                await userManager.CreateAsync(stephen, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(stephen, new Claim("IsAdmin", "true"));
                await userManager.AddClaimAsync(stephen, new Claim("IsNurse", "true"));
            }

            // Ensure Jerry (IsAdmin)
            var jerry = await userManager.FindByNameAsync("jerry@cc.com");
            if (jerry == null)
            {
                // create user
                jerry = new ApplicationUser
                {
                    UserName = "jerry@cc.com",
                    Email = "jerry@cc.com",
                    FirstName = "Jerry",
                    LastName = "Morris",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 6, 28, 12, 45, 15)
                };
                await userManager.CreateAsync(jerry, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(jerry, new Claim("IsAdmin", "true"));
                await userManager.AddClaimAsync(jerry, new Claim("IsNurse", "true"));
            }

            // Ensure Sharon (IsAdmin)
            var sharon = await userManager.FindByNameAsync("sharon@cc.com");
            if (sharon == null)
            {
                // create user
                sharon = new ApplicationUser
                {
                    UserName = "sharon@cc.com",
                    Email = "sharonr@cc.com",
                    FirstName = "Sharon",
                    LastName = "Kuan",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 6, 28, 12, 45, 15)
                };
                await userManager.CreateAsync(sharon, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(sharon, new Claim("IsAdmin", "true"));
                await userManager.AddClaimAsync(sharon, new Claim("IsNurse", "true"));
            }

            // Ensure Reno (IsAdmin)
            var reno = await userManager.FindByNameAsync("reno@cc.com");
            if (reno == null)
            {
                // create user
                reno = new ApplicationUser
                {
                    UserName = "reno@cc.com",
                    Email = "reno@cc.com",
                    FirstName = "Reno",
                    LastName = "McGalla",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 6, 28, 12, 45, 15)
                };
                await userManager.CreateAsync(reno, "Secret123!");

                // add claims
                await userManager.AddClaimAsync(reno, new Claim("IsAdmin", "true"));
                await userManager.AddClaimAsync(reno, new Claim("IsNurse", "true"));
            }


            // Ensure Mike (not IsAdmin)
            var mike = await userManager.FindByNameAsync("Mike@CoderCamps.com");
            if (mike == null)
            {
                // create user
                mike = new ApplicationUser
                {
                    UserName = "Mike@CoderCamps.com",
                    Email = "Mike@CoderCamps.com",
                    FirstName = "Mike",
                    LastName = "White",
                    IsActive = true,
                    DateModifiedIfActive = new DateTime(2016, 6, 28, 12, 45, 15)
                };
                await userManager.CreateAsync(mike, "Secret123!");
            }

        }

    }
}
