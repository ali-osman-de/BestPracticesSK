<script setup lang="ts">
import { ref } from 'vue'

type Role = 'user' | 'assistant'
type Message = { role: Role; content: string }

const messages = ref<Message[]>([])
const input = ref('')
const loading = ref(false)

async function sendMessage() {
  const text = input.value.trim()
  if (!text || loading.value) return

  messages.value.push({ role: 'user', content: text })
  input.value = ''

  const assistant: Message = { role: 'assistant', content: '' }
  messages.value.push(assistant)

  loading.value = true
  try {
    const base = (import.meta as any).env?.VITE_API_BASE || 'https://localhost:7010'
    const url = `${String(base).replace(/\/$/, '')}/api/Chat/message?userMessage=${encodeURIComponent(text)}`

    const resp = await fetch(url, {
      method: 'POST',
      headers: {
        'Accept': 'text/plain'
      }
    })

    if (!resp.ok) {
      const errText = await resp.text().catch(() => '')
      throw new Error(`HTTP ${resp.status} ${resp.statusText} ${errText}`)
    }

    const result = await resp.text()
    assistant.content = result

    requestAnimationFrame(() => {
      const el = document.getElementById('messages')
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (err: any) {
    messages.value.push({ role: 'assistant', content: '❌ Hata: ' + (err?.message || 'Bilinmeyen hata') })
  } finally {
    loading.value = false
  }
}


function onEnter(e: KeyboardEvent) {
  if (e.key === 'Enter' && !e.shiftKey) {
    e.preventDefault()
    sendMessage()
  }
}
</script>

<template>
  <div class="chat">
    <header class="chat-header">💬 Basit Chatbot</header>

    <div class="messages" id="messages">
      <div
        v-for="(m, i) in messages"
        :key="i"
        class="message"
        :class="m.role"
      >
        <div class="bubble">
          {{ m.content }}
          <span
            v-if="loading && i === messages.length - 1 && m.role === 'assistant'"
            class="cursor"
            aria-label="typing"
          />
        </div>
      </div>
    </div>

    <div class="input-row">
      <textarea
        v-model="input"
        class="input"
        placeholder="Mesajınızı yazın..."
        rows="2"
        @keydown="onEnter"
      />
      <button class="send" :disabled="loading || !input.trim()" @click="sendMessage">Gönder</button>
    </div>
  </div>
</template>

<style scoped>
.chat {
  max-width: 800px;
  margin: 40px auto;
  padding: 0;
  display: flex;
  flex-direction: column;
  height: 80vh;
  border: 1px solid #e5e7eb;
  border-radius: 16px;
  overflow: hidden;
  background: #fff;
  box-shadow: 0 4px 16px rgba(0,0,0,0.1);
}

.chat-header {
  background: #4f46e5;
  color: #fff;
  text-align: center;
  font-size: 18px;
  font-weight: 600;
  padding: 12px;
  position: sticky;
  top: 0;
}

.messages {
  flex: 1;
  overflow-y: auto;
  padding: 16px;
  background: #f9fafb;
}

.message {
  display: flex;
  margin: 8px 0;
}

.message.user {
  justify-content: flex-end;
}

.message.assistant {
  justify-content: flex-start;
}

.bubble {
  max-width: 70%;
  padding: 10px 14px;
  border-radius: 16px;
  white-space: pre-wrap;
  font-size: 15px;
  line-height: 1.4;
  box-shadow: 0 2px 6px rgba(0,0,0,0.08);
}

.user .bubble {
  background: #4f46e5;
  color: white;
  border-bottom-right-radius: 4px;
}

.assistant .bubble {
  background: white;
  border: 1px solid #e5e7eb;
  border-bottom-left-radius: 4px;
}

.cursor {
  display: inline-block;
  width: 6px;
  height: 1em;
  margin-left: 4px;
  background: #4f46e5;
  vertical-align: text-bottom;
  animation: blink 1s steps(1) infinite;
}

@keyframes blink {
  50% { opacity: 0; }
}

.input-row {
  display: flex;
  gap: 8px;
  padding: 12px;
  border-top: 1px solid #e5e7eb;
  background: #fff;
}

.input {
  flex: 1;
  padding: 10px;
  border-radius: 12px;
  border: 1px solid #ddd;
  resize: none;
  font-size: 14px;
}

.send {
  padding: 10px 16px;
  border-radius: 12px;
  border: none;
  background: #4f46e5;
  color: #fff;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.2s;
}

.send:hover:not(:disabled) {
  background: #4338ca;
}

.send:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
