<template>
    <div class="container">
      <h1>Product List</h1>
  
      <!-- Add Form -->
      <div class="add-form">
        <input
          v-model="newCode"
          placeholder="xxxx-xxxx-xxxx-xxxx"
          maxlength="19"
          @input="formatCode"
        />
        <span v-if="codeError" class="error">{{ codeError }}</span>
        <button @click="addProduct">Add</button>
      </div>
  
      <!-- Table -->
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Product code</th>
            <th>Barcode</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="product in products" :key="product.id">
            <td>{{ product.id }}</td>
            <td>{{ product.code }}</td>
            <td>
              <svg :id="`barcode-${product.id}`"></svg>
            </td>
            <td>
              <button @click="deleteProduct(product.id)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
  
      <!-- Pagination -->
      <div class="pagination">
        <button :disabled="page === 1" @click="changePage(page - 1)">Previous</button>
        <span>Page {{ page }} / {{ totalPages }}</span>
        <button :disabled="page === totalPages" @click="changePage(page + 1)">Next</button>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted, nextTick } from 'vue'
  import JsBarcode from 'jsbarcode'
  import api from '@/services/api'
  
  const products = ref<any[]>([])
  const page = ref(1)
  const totalPages = ref(1)
  const newCode = ref('')
  const codeError = ref('')

  const renderBarcodes = async () => {
    await nextTick()
    setTimeout(() => {
      products.value.forEach(product => {
        const el = document.getElementById(`barcode-${product.id}`)
        if (el) {
          JsBarcode(el, product.code.replace(/-/g, ''), {
            format: 'CODE39',
            height: 40,
            fontSize: 12,
            displayValue: true
          })
        }
      })
    }, 100)
  }

  const fetchProducts = async () => {
    const res = await api.get(`/products?page=${page.value}&pageSize=25`)
    products.value = res.data.data.items
    totalPages.value = res.data.data.pagination.totalPages
    await renderBarcodes()
  }
  
  const formatCode = () => {
    // auto format xxxx-xxxx-xxxx-xxxx
    let val = newCode.value.replace(/[^A-Z0-9]/g, '').toUpperCase()
    val = val.match(/.{1,4}/g)?.join('-') ?? val
    newCode.value = val
  }
  
  const validateCode = (code: string) => {
    const regex = /^[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}$/
    return regex.test(code)
  }
  
  const addProduct = async () => {
    codeError.value = ''
    if (!validateCode(newCode.value)) {
      codeError.value = 'รหัสสินค้าต้องเป็น A-Z หรือ 0-9 เท่านั้น และมี 16 หลัก'
      return
    }
    
    try {
      await api.post('/products', {
        code: newCode.value,
        name: newCode.value,
        categoryId: 1,
        price: 0,
        stock: 0,
        createdBy: 'admin'
      })
      newCode.value = ''
      await fetchProducts()
      alert('เพิ่มสินค้าสำเร็จ ✅')
    } catch (error: any) {
      if (error.response?.status === 409) {
        codeError.value = 'รหัสสินค้านี้มีอยู่แล้วค่ะ'
      } else {
        codeError.value = 'เกิดข้อผิดพลาด กรุณาลองใหม่ค่ะ'
      }
    }
  }

  const deleteProduct = async (id: number) => {
    const confirmed = confirm('ยืนยันการลบสินค้านี้?')
    if (!confirmed) return
    
    try {
      await api.delete(`/products/${id}`)
      await fetchProducts()
      alert('ลบสินค้าสำเร็จ ✅')
    } catch {
      alert('เกิดข้อผิดพลาด กรุณาลองใหม่ค่ะ')
    }
  }
  
  const changePage = async (newPage: number) => {
    page.value = newPage
    await fetchProducts()
  }
  
  onMounted(fetchProducts)
  </script>
  
  <style scoped>
  .container { padding: 24px; }
  .add-form { display: flex; gap: 8px; margin-bottom: 16px; align-items: center;}
  .add-form input {padding:4px}
  .error { color: red; font-size: 12px; }
  table { width: 100%; border-collapse: collapse; }
  th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
  .pagination { display: flex; gap: 8px; margin-top: 16px; align-items: center; }
  button { cursor: pointer; padding: 4px 12px; }
  button:disabled { opacity: 0.5; cursor: not-allowed; }
  </style>