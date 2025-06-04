"use client";

import { useState } from "react";
import { Button } from "../../components/ui/button";
import { Input } from "../../components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../../components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "../../components/ui/dropdown-menu";
import { ChevronDown, ChevronUp, MoreHorizontal, Plus } from "lucide-react";
import { Badge } from "../../components/ui/badge";

const guests = [
  {
    id: 1,
    name: "John Doe",
    email: "john@example.com",
    rsvp: "Yes",
    plusOne: "No",
    dietaryRestrictions: "None",
    tableAssignment: "Table 1",
  },
  {
    id: 2,
    name: "Jane Smith",
    email: "jane@example.com",
    rsvp: "No",
    plusOne: "N/A",
    dietaryRestrictions: "Vegetarian",
    tableAssignment: "Unassigned",
  },
  {
    id: 3,
    name: "Bob Johnson",
    email: "bob@example.com",
    rsvp: "Pending",
    plusOne: "Yes",
    dietaryRestrictions: "Gluten-free",
    tableAssignment: "Table 2",
  },
  {
    id: 4,
    name: "Alice Brown",
    email: "alice@example.com",
    rsvp: "Yes",
    plusOne: "Yes",
    dietaryRestrictions: "None",
    tableAssignment: "Table 1",
  },
  {
    id: 5,
    name: "Charlie Davis",
    email: "charlie@example.com",
    rsvp: "Pending",
    plusOne: "No",
    dietaryRestrictions: "Nut allergy",
    tableAssignment: "Unassigned",
  },
  // Add more guest data as needed
];

type SortKey = "name" | "email" | "rsvp" | "plusOne" | "tableAssignment";

export default function Guests() {
  const [searchTerm, setSearchTerm] = useState("");
  const [sortKey, setSortKey] = useState<SortKey>("name");
  const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;

  const filteredGuests = guests.filter(
    (guest) =>
      guest.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      guest.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const sortedGuests = [...filteredGuests].sort((a, b) => {
    if (a[sortKey] < b[sortKey]) return sortOrder === "asc" ? -1 : 1;
    if (a[sortKey] > b[sortKey]) return sortOrder === "asc" ? 1 : -1;
    return 0;
  });

  const paginatedGuests = sortedGuests.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  const totalPages = Math.ceil(sortedGuests.length / itemsPerPage);

  const handleSort = (key: SortKey) => {
    if (key === sortKey) {
      setSortOrder(sortOrder === "asc" ? "desc" : "asc");
    } else {
      setSortKey(key);
      setSortOrder("asc");
    }
  };

  const getRSVPBadge = (rsvp: string) => {
    switch (rsvp) {
      case "Yes":
        return <Badge className='bg-green-500'>Yes</Badge>;
      case "No":
        return <Badge className='bg-red-500'>No</Badge>;
      case "Pending":
        return <Badge className='bg-yellow-500'>Pending</Badge>;
      default:
        return null;
    }
  };

  return (
    <div className='p-6'>
      <div className='flex justify-between items-center mb-6'>
        <h1 className='text-3xl font-bold'>Guests</h1>
        <Button>
          <Plus className='mr-2 h-4 w-4' /> Add New Guest
        </Button>
      </div>
      <div className='mb-4 flex justify-between items-center'>
        <Input
          type='text'
          placeholder='Search guests...'
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className='max-w-sm'
        />
        <div className='text-sm text-gray-500'>
          Showing {paginatedGuests.length} of {sortedGuests.length} guests
        </div>
      </div>
      <div className='rounded-md border'>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead className='w-[200px]'>
                <button
                  className='flex items-center'
                  onClick={() => handleSort("name")}>
                  Name
                  {sortKey === "name" &&
                    (sortOrder === "asc" ? (
                      <ChevronUp className='ml-2 h-4 w-4' />
                    ) : (
                      <ChevronDown className='ml-2 h-4 w-4' />
                    ))}
                </button>
              </TableHead>
              <TableHead>
                <button
                  className='flex items-center'
                  onClick={() => handleSort("email")}>
                  Email
                  {sortKey === "email" &&
                    (sortOrder === "asc" ? (
                      <ChevronUp className='ml-2 h-4 w-4' />
                    ) : (
                      <ChevronDown className='ml-2 h-4 w-4' />
                    ))}
                </button>
              </TableHead>
              <TableHead>
                <button
                  className='flex items-center'
                  onClick={() => handleSort("rsvp")}>
                  RSVP
                  {sortKey === "rsvp" &&
                    (sortOrder === "asc" ? (
                      <ChevronUp className='ml-2 h-4 w-4' />
                    ) : (
                      <ChevronDown className='ml-2 h-4 w-4' />
                    ))}
                </button>
              </TableHead>
              <TableHead>
                <button
                  className='flex items-center'
                  onClick={() => handleSort("plusOne")}>
                  Plus One
                  {sortKey === "plusOne" &&
                    (sortOrder === "asc" ? (
                      <ChevronUp className='ml-2 h-4 w-4' />
                    ) : (
                      <ChevronDown className='ml-2 h-4 w-4' />
                    ))}
                </button>
              </TableHead>
              <TableHead>
                <button
                  className='flex items-center'
                  onClick={() => handleSort("tableAssignment")}>
                  Table Assignment
                  {sortKey === "tableAssignment" &&
                    (sortOrder === "asc" ? (
                      <ChevronUp className='ml-2 h-4 w-4' />
                    ) : (
                      <ChevronDown className='ml-2 h-4 w-4' />
                    ))}
                </button>
              </TableHead>
              <TableHead className='text-right'>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {paginatedGuests.map((guest) => (
              <TableRow key={guest.id}>
                <TableCell className='font-medium'>{guest.name}</TableCell>
                <TableCell>{guest.email}</TableCell>
                <TableCell>{getRSVPBadge(guest.rsvp)}</TableCell>
                <TableCell>{guest.plusOne}</TableCell>
                <TableCell>{guest.tableAssignment}</TableCell>
                <TableCell className='text-right'>
                  <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                      <Button variant='ghost' className='h-8 w-8 p-0'>
                        <span className='sr-only'>Open menu</span>
                        <MoreHorizontal className='h-4 w-4' />
                      </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align='end'>
                      <DropdownMenuLabel>Actions</DropdownMenuLabel>
                      <DropdownMenuItem
                        onClick={() =>
                          navigator.clipboard.writeText(guest.email)
                        }>
                        Copy email
                      </DropdownMenuItem>
                      <DropdownMenuSeparator />
                      <DropdownMenuItem>Edit guest</DropdownMenuItem>
                      <DropdownMenuItem>Delete guest</DropdownMenuItem>
                    </DropdownMenuContent>
                  </DropdownMenu>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
      <div className='flex items-center justify-end space-x-2 py-4'>
        <Button
          variant='outline'
          size='sm'
          onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
          disabled={currentPage === 1}>
          Previous
        </Button>
        <Button
          variant='outline'
          size='sm'
          onClick={() =>
            setCurrentPage((prev) => Math.min(prev + 1, totalPages))
          }
          disabled={currentPage === totalPages}>
          Next
        </Button>
      </div>
    </div>
  );
}
