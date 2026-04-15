import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import PageLayout from './components/layout/PageLayout'
import FormatterPage from './pages/FormatterPage'
import EncodingPage from './pages/EncodingPage'
import TextToolsPage from './pages/TextToolsPage'
import DiffPage from './pages/DiffPage'
import TimePage from './pages/TimePage'

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<PageLayout />}>
          <Route index element={<Navigate to="/formatter" replace />} />
          <Route path="formatter" element={<FormatterPage />} />
          <Route path="encode" element={<EncodingPage />} />
          <Route path="text" element={<TextToolsPage />} />
          <Route path="diff" element={<DiffPage />} />
          <Route path="time" element={<TimePage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
