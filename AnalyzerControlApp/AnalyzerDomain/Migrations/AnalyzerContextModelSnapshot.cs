﻿// <auto-generated />
using System;
using AnalyzerDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnalyzerDomain.Migrations
{
    [DbContext(typeof(AnalyzerContext))]
    partial class AnalyzerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.14");

            modelBuilder.Entity("AnalyzerDomain.Models.AnalysisStage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("IncubationTimeInMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NeedIncubation")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NeedWashStep")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfWashStep")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PipettingVolume")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AnalysisStage");
                });

            modelBuilder.Entity("AnalyzerDomain.Models.AnalysisType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CartridgeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ConjugateStageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int?>("EnzymeComplexStageId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SamplingStageId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SubstrateStageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CartridgeId");

                    b.HasIndex("ConjugateStageId");

                    b.HasIndex("EnzymeComplexStageId");

                    b.HasIndex("SamplingStageId");

                    b.HasIndex("SubstrateStageId");

                    b.ToTable("AnalysisTypes");
                });

            modelBuilder.Entity("AnalyzerDomain.Models.Cartridge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cartridges");
                });

            modelBuilder.Entity("AnalyzerDomain.Models.AnalysisType", b =>
                {
                    b.HasOne("AnalyzerDomain.Models.Cartridge", "Cartridge")
                        .WithMany("AnalyzesTypes")
                        .HasForeignKey("CartridgeId");

                    b.HasOne("AnalyzerDomain.Models.AnalysisStage", "ConjugateStage")
                        .WithMany()
                        .HasForeignKey("ConjugateStageId");

                    b.HasOne("AnalyzerDomain.Models.AnalysisStage", "EnzymeComplexStage")
                        .WithMany()
                        .HasForeignKey("EnzymeComplexStageId");

                    b.HasOne("AnalyzerDomain.Models.AnalysisStage", "SamplingStage")
                        .WithMany()
                        .HasForeignKey("SamplingStageId");

                    b.HasOne("AnalyzerDomain.Models.AnalysisStage", "SubstrateStage")
                        .WithMany()
                        .HasForeignKey("SubstrateStageId");
                });
#pragma warning restore 612, 618
        }
    }
}
