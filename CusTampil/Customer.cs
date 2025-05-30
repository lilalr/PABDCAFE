﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CusTampil
{
    public partial class Customer : Form
    {
        private string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe";
        private DataTable customerTable;

        public Customer()
        {
            InitializeComponent();
            InitializeTable();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void InitializeTable()
        {
            customerTable = new DataTable();
            customerTable.Columns.Add("Nama");
            customerTable.Columns.Add("No Telp");
            customerTable.Columns.Add("Pilih Meja");
            customerTable.Columns.Add("Waktu Reservasi");
        }


        private void ClearForm()
        {
            txtCus1.Clear();
            txtCus2.Clear();
            txtCus3.Clear();
            txtCus4.Clear();
            txtCus1.Focus();

            // fokus kembali NAMA agar user memasukkan data baru
            label1.Focus();
        }

        private void LoadData()
        {
            dgvCus.AutoGenerateColumns = true;
            dgvCus.DataSource = customerTable;
            ClearForm();
        }

        private void btnSubmit(object sender, EventArgs e)
        {
            if (txtCus1.Text == "" || txtCus2.Text == "" || txtCus3.Text == "" || txtCus4.Text == "")
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            customerTable.Rows.Add(txtCus1.Text.Trim(), txtCus2.Text.Trim(), txtCus3.Text.Trim(), txtCus4.Text.Trim());

            MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadData();
        }

        private void btnHapus(object sender, EventArgs e)
        {
            // Pastikan ada baris yang dipilih
            if (dgvCus.CurrentRow == null)
            {
                MessageBox.Show("Pilih satu baris terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tanyakan konfirmasi penghapusan
            var result = MessageBox.Show(
                "Apakah Anda yakin ingin menghapus data ini?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                // Hapus baris terpilih dari DataTable
                int idx = dgvCus.CurrentRow.Index;
                customerTable.Rows.RemoveAt(idx);

                MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
        }
    }
}