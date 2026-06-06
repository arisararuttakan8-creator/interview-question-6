import axios from 'axios'

const api = axios.create({
  baseURL: 'https://interview-question-6-production.up.railway.app/api'
})

export default api