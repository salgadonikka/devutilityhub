import { Outlet } from 'react-router-dom'
import Header from './Header'
import Sidebar from './Sidebar'

export default function PageLayout() {
  return (
    <div className="flex h-screen overflow-hidden bg-(--t-bg)">
      {/* CRT scanline overlay — pointer-events-none so it never blocks interaction */}
      <div
        className="pointer-events-none fixed inset-0 z-50 scanlines"
        aria-hidden="true"
      />

      <Sidebar />

      <div className="flex flex-col flex-1 min-w-0">
        <Header />
        <main className="flex-1 overflow-y-auto p-5 bg-(--t-bg)">
          <Outlet />
        </main>
      </div>
    </div>
  )
}
