import React, { useState, useEffect } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { hasRole } from '../roles';
import { clearAuth } from '../dataProvider';

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

interface MenuItem {
  label: string;
  path: string;
  icon?: string;
  requiredRoles?: string[];
  children?: MenuItem[];
}

/** Filter menu items by user roles (recursive for groups) */
const filterByRole = (items: MenuItem[]): MenuItem[] =>
  items
    .filter((item) => hasRole(item.requiredRoles ?? []))
    .map((item) =>
      item.children ? { ...item, children: filterByRole(item.children) } : item
    )
    .filter((item) => !item.children || item.children.length > 0);

// ---------------------------------------------------------------------------
// Menu Configuration
// ---------------------------------------------------------------------------

const MENU_ITEMS: MenuItem[] = [
  {
    "label": "My Tasks",
    "path": "/tasks",
    "icon": "task_alt"
  },
  {
    "label": "Dashboard",
    "path": "/dashboard",
    "icon": "dashboard"
  },
  {
    "label": "Birim",
    "path": "/Department",
    "icon": "article"
  },
  {
    "label": "Personel",
    "path": "/Employee",
    "icon": "article"
  },
  {
    "label": "Tedarikçi",
    "path": "/Supplier",
    "icon": "article"
  },
  {
    "label": "Harcama Kalemi",
    "path": "/ExpenseCategory",
    "icon": "category"
  },
  {
    "label": "Satın Alma Talebi",
    "path": "/PurchaseRequest",
    "icon": "shopping_cart"
  },
  {
    "label": "Talep Kalemi",
    "path": "/PurchaseRequestItem",
    "icon": "inventory_2"
  },
  {
    "label": "onur",
    "path": "",
    "icon": "folder",
    "children": [
      {
        "label": "Sipariş",
        "path": "/PurchaseOrder",
        "icon": "shopping_cart"
      },
      {
        "label": "Teklif",
        "path": "/Quotation",
        "icon": "article"
      }
    ]
  },
  {
    "label": "Administration",
    "path": "#",
    "icon": "admin_panel_settings",
    "requiredRoles": [
      "Admin"
    ],
    "children": [
      {
        "label": "Users",
        "path": "/users",
        "icon": "people"
      },
      {
        "label": "Roles",
        "path": "/roles",
        "icon": "security"
      }
    ]
  }
];

// ---------------------------------------------------------------------------
// Icon Component (Material Symbols)
// ---------------------------------------------------------------------------

const Icon: React.FC<{ name?: string; size?: number; className?: string }> = ({ name, size = 20, className }) => {
  if (!name) return null;
  return (
    <span
      className={`material-symbols-outlined ${className ?? ''}`}
      style={{ fontSize: size, lineHeight: 1, flexShrink: 0, userSelect: 'none' }}
    >
      {name}
    </span>
  );
};

// ---------------------------------------------------------------------------
// Menu Item Components
// ---------------------------------------------------------------------------

const MenuLink: React.FC<{
  item: MenuItem;
  collapsed: boolean;
  depth?: number;
}> = ({ item, collapsed, depth = 0 }) => (
  <NavLink
    to={item.path}
    title={collapsed ? item.label : undefined}
    style={({ isActive }) => ({
      display: 'flex',
      alignItems: 'center',
      gap: collapsed ? 0 : 10,
      padding: collapsed ? '10px 0' : `9px ${depth > 0 ? 12 : 12}px 9px ${12 + depth * 16}px`,
      borderRadius: 6,
      textDecoration: 'none',
      fontSize: depth > 0 ? 13 : 14,
      marginBottom: 2,
      justifyContent: collapsed ? 'center' : 'flex-start',
      background: isActive ? 'var(--primary-50)' : 'transparent',
      color: isActive ? 'var(--primary-500)' : '#555555',
      fontWeight: isActive ? 600 : 400,
      transition: 'background 0.15s, color 0.15s',
    })}
  >
    <Icon name={item.icon ?? (depth > 0 ? 'circle' : 'article')} size={depth > 0 ? 16 : 20} />
    {!collapsed && <span style={{ overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{item.label}</span>}
  </NavLink>
);

const MenuGroup: React.FC<{
  item: MenuItem;
  collapsed: boolean;
}> = ({ item, collapsed }) => {
  const location = useLocation();
  const isChildActive = item.children?.some((c) => location.pathname.startsWith(c.path)) ?? false;
  const [open, setOpen] = useState(isChildActive);

  useEffect(() => {
    if (isChildActive && !open) setOpen(true);
  }, [isChildActive]);

  // Close popup when switching between collapsed/expanded
  useEffect(() => {
    if (collapsed) setOpen(false);
  }, [collapsed]);

  const iconRef = React.useRef<HTMLDivElement>(null);
  const popupRef = React.useRef<HTMLDivElement>(null);
  const [popupPos, setPopupPos] = React.useState({ top: 0, left: 0 });

  React.useEffect(() => {
    if (!open || !collapsed) return;
    // Position popup next to icon
    if (iconRef.current) {
      const rect = iconRef.current.getBoundingClientRect();
      setPopupPos({ top: rect.top, left: rect.right + 4 });
    }
    // Close on outside click
    const handler = (e: MouseEvent) => {
      if (popupRef.current && !popupRef.current.contains(e.target as Node) &&
          iconRef.current && !iconRef.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener('mousedown', handler);
    return () => document.removeEventListener('mousedown', handler);
  }, [open, collapsed]);

  if (collapsed) {
    return (
      <div title={item.label}>
        <div
          ref={iconRef}
          style={{
            display: 'flex',
            justifyContent: 'center',
            padding: '10px 0',
            cursor: 'pointer',
            color: isChildActive ? 'var(--primary-500)' : '#555555',
            borderRadius: 6,
            background: isChildActive ? 'var(--primary-50)' : 'transparent',
            marginBottom: 2,
          }}
          onClick={() => setOpen(!open)}
        >
          <Icon name={item.icon ?? 'folder'} />
        </div>
        {open && (
          <div ref={popupRef} style={{
            position: 'fixed', top: popupPos.top, left: popupPos.left,
            background: '#fff', borderRadius: 8, border: '1px solid #e0e0e0',
            boxShadow: '0 4px 16px rgba(0,0,0,0.12)', minWidth: 180,
            zIndex: 99999, overflow: 'hidden', padding: '4px 0',
          }}>
            <div style={{ padding: '6px 12px', fontSize: 11, color: '#999', fontWeight: 600, textTransform: 'uppercase' }}>
              {item.label}
            </div>
            {item.children?.map((child) => (
              <div key={child.path} onClick={() => setOpen(false)}>
                <MenuLink item={child} collapsed={false} depth={0} />
              </div>
            ))}
          </div>
        )}
      </div>
    );
  }

  return (
    <div>
      <div
        onClick={() => setOpen(!open)}
        style={{
          display: 'flex',
          alignItems: 'center',
          gap: 10,
          padding: '9px 12px',
          borderRadius: 6,
          cursor: 'pointer',
          fontSize: 14,
          marginBottom: 2,
          color: isChildActive ? 'var(--primary-500)' : '#555555',
          fontWeight: isChildActive ? 600 : 400,
          background: isChildActive ? 'var(--primary-50)' : 'transparent',
          transition: 'background 0.15s',
        }}
      >
        <Icon name={item.icon ?? 'folder'} />
        <span style={{ flex: 1, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>{item.label}</span>
        <Icon name={open ? 'expand_less' : 'expand_more'} size={18} />
      </div>
      {open && (
        <div>
          {item.children?.map((child) => (
            <MenuLink key={child.path} item={child} collapsed={false} depth={1} />
          ))}
        </div>
      )}
    </div>
  );
};

// ---------------------------------------------------------------------------
// GlobalMenu Component
// ---------------------------------------------------------------------------

// ---------------------------------------------------------------------------
// User Dropdown — top-right user menu with logout
// ---------------------------------------------------------------------------

const UserDropdown: React.FC = () => {
  const [open, setOpen] = useState(false);
  const ref = React.useRef<HTMLDivElement>(null);
  const btnRef = React.useRef<HTMLButtonElement>(null);
  const [pos, setPos] = React.useState({ top: 0, right: 0 });

  React.useEffect(() => {
    if (!open) return;
    const handler = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node) &&
          btnRef.current && !btnRef.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener('mousedown', handler);
    return () => document.removeEventListener('mousedown', handler);
  }, [open]);

  const handleToggle = () => {
    if (!open && btnRef.current) {
      const rect = btnRef.current.getBoundingClientRect();
      setPos({ top: rect.bottom + 4, right: window.innerWidth - rect.right });
    }
    setOpen(!open);
  };

  // Try to get user info from localStorage (set after login)
  const userName = localStorage.getItem('_user_name') || 'User';
  const userEmail = localStorage.getItem('_user_email') || '';
  const initials = userName.slice(0, 2).toUpperCase();

  return (
    <div style={{ position: 'relative' }}>
      <button
        ref={btnRef}
        onClick={handleToggle}
        style={{
          display: 'flex', alignItems: 'center', gap: 8,
          padding: '4px 12px', borderRadius: 8,
          border: '1px solid #e0e0e0', background: '#fff',
          cursor: 'pointer', fontSize: 13, color: '#333',
        }}
      >
        <div style={{
          width: 28, height: 28, borderRadius: '50%',
          background: 'var(--primary-500)', color: '#fff',
          display: 'flex', alignItems: 'center', justifyContent: 'center',
          fontSize: 11, fontWeight: 700,
        }}>
          {initials}
        </div>
        <span style={{ maxWidth: 120, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
          {userName}
        </span>
        <Icon name={open ? 'expand_less' : 'expand_more'} size={16} />
      </button>

      {open && (
        <div ref={ref} style={{
          position: 'fixed', top: pos.top, right: pos.right,
          background: '#fff', borderRadius: 8, border: '1px solid #e0e0e0',
          boxShadow: '0 4px 16px rgba(0,0,0,0.1)', minWidth: 200,
          zIndex: 10000, overflow: 'hidden',
        }}>
          {userEmail && (
            <div style={{ padding: '12px 16px', borderBottom: '1px solid #f0f0f0', fontSize: 12, color: '#999' }}>
              {userEmail}
            </div>
          )}
          <UserMenuItem icon="person" label="Profile" onClick={() => setOpen(false)} />
          <UserMenuItem icon="settings" label="Settings" onClick={() => setOpen(false)} />
          <div style={{ height: 1, background: '#f0f0f0' }} />
          <UserMenuItem icon="logout" label="Logout" onClick={() => { clearAuth(); window.location.reload(); setOpen(false); }} danger />
        </div>
      )}
    </div>
  );
};

const UserMenuItem: React.FC<{ icon: string; label: string; onClick: () => void; danger?: boolean }> = ({ icon, label, onClick, danger }) => (
  <button
    onClick={onClick}
    style={{
      display: 'flex', alignItems: 'center', gap: 10, width: '100%',
      padding: '10px 16px', border: 'none', background: 'none',
      cursor: 'pointer', fontSize: 13, color: danger ? '#d32f2f' : '#333',
      textAlign: 'left',
    }}
    onMouseEnter={(e) => { (e.currentTarget).style.background = '#f5f5f5'; }}
    onMouseLeave={(e) => { (e.currentTarget).style.background = 'none'; }}
  >
    <Icon name={icon} size={18} />
    {label}
  </button>
);

// ---------------------------------------------------------------------------
// GlobalMenu
// ---------------------------------------------------------------------------

export const GlobalMenu: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [collapsed, setCollapsed] = useState(false);

  // Auto-collapse sidebar on small screens instead of switching to mobile drawer
  useEffect(() => {
    const onResize = () => {
      if (window.innerWidth < 768) setCollapsed(true);
    };
    onResize();
    window.addEventListener('resize', onResize);
    return () => window.removeEventListener('resize', onResize);
  }, []);

  const sidebarWidth = collapsed ? 64 : 240;

  const sidebar = (
    <aside
      style={{
        width: sidebarWidth,
        height: '100dvh',
        borderRight: '1px solid var(--tof-color-border, #e8e8e8)',
        background: '#ffffff',
        display: 'flex',
        flexDirection: 'column',
        flexShrink: 0,
        transition: 'width 0.2s ease',
        overflow: 'hidden',
      }}
    >
      {/* Header */}
      <div
        style={{
          padding: collapsed ? '16px 8px' : '16px 12px',
          display: 'flex',
          alignItems: 'center',
          justifyContent: collapsed ? 'center' : 'space-between',
          minHeight: 56,
          flexShrink: 0,
        }}
      >
        {!collapsed && (
          <div style={{
            fontWeight: 700,
            fontSize: 17,
            color: 'var(--primary-500)',
            display: 'flex',
            alignItems: 'center',
            overflow: 'hidden',
            whiteSpace: 'nowrap',
          }}>
            <img src="data:image/png;base64,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" alt="" style={{ height: 24, marginRight: 8 }} />{!collapsed && "DepartmentApp2"}
          </div>
        )}
        <button
          onClick={() => setCollapsed(!collapsed)}
          title={collapsed ? 'Expand menu' : 'Collapse menu'}
          style={{
            background: 'none',
            border: 'none',
            cursor: 'pointer',
            padding: 4,
            borderRadius: 4,
            color: '#555555',
            display: 'flex',
            alignItems: 'center',
          }}
        >
          <Icon name={collapsed ? 'menu_open' : 'menu'} size={22} />
        </button>
      </div>

      {/* Navigation */}
      <nav style={{ flex: 1, overflow: 'auto', padding: collapsed ? '0 8px' : '0 8px' }}>
        {filterByRole(MENU_ITEMS).map((item) =>
          item.children && item.children.length > 0 ? (
            <MenuGroup key={item.label} item={item} collapsed={collapsed} />
          ) : (
            <MenuLink key={item.path} item={item} collapsed={collapsed} />
          )
        )}
      </nav>

      {/* Footer — version info */}
      <div style={{ flexShrink: 0, padding: '8px 12px', borderTop: '1px solid #e8e8e8', fontSize: 10, color: '#bbb', textAlign: 'center' }}>
        Powered by Archipid
      </div>
    </aside>
  );

  return (
    <div style={{ display: 'flex', height: '100dvh', overflow: 'hidden', fontFamily: 'var(--tof-font-family, system-ui, sans-serif)' }}>
      {/* Sidebar — always inline, collapses to icon-only on small screens */}
      {sidebar}

      {/* Main content */}
      <div style={{ flex: 1, display: 'flex', flexDirection: 'column', overflow: 'hidden', minWidth: 0, minHeight: 0 }}>
        {/* Top header bar */}
        <div style={{
          display: 'flex', alignItems: 'center', justifyContent: 'flex-end',
          padding: '8px 24px', borderBottom: '1px solid #e8e8e8', background: '#fff',
          flexShrink: 0, gap: 12,
        }}>
          <UserDropdown />
        </div>

        <main style={{ flex: 1, overflow: 'auto', padding: '32px 36px', background: 'var(--tof-color-bg-subtle, #f8f9fa)' }}>
          {children}
        </main>
      </div>
    </div>
  );
};
