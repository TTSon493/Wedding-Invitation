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
import { Badge } from "../../components/ui/badge";

const rsvpData = [
  {
    id: 1,
    name: "John Doe",
    email: "john@example.com",
    status: "Attending",
    guests: 2,
    dietaryRestrictions: "None",
  },
  {
    id: 2,
    name: "Jane Smith",
    email: "jane@example.com",
    status: "Not Attending",
    guests: 0,
    dietaryRestrictions: "N/A",
  },
  {
    id: 3,
    name: "Bob Johnson",
    email: "bob@example.com",
    status: "Pending",
    guests: 1,
    dietaryRestrictions: "Vegetarian",
  },
  // Add more RSVP data as needed
];

export default function RSVPManagement() {
  const [searchTerm, setSearchTerm] = useState("");

  const filteredRSVPs = rsvpData.filter(
    (rsvp) =>
      rsvp.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
      rsvp.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Attending":
        return <Badge className='bg-green-500'>Attending</Badge>;
      case "Not Attending":
        return <Badge className='bg-red-500'>Not Attending</Badge>;
      case "Pending":
        return <Badge className='bg-yellow-500'>Pending</Badge>;
      default:
        return null;
    }
  };

  return (
    <div className='p-6'>
      <div className='flex justify-between items-center mb-6'>
        <h1 className='text-3xl font-bold'>RSVP Management</h1>
        <Button>Send Reminders</Button>
      </div>
      <div className='mb-4'>
        <Input
          type='text'
          placeholder='Search RSVPs...'
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className='max-w-sm'
        />
      </div>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Name</TableHead>
            <TableHead>Email</TableHead>
            <TableHead>Status</TableHead>
            <TableHead>Guests</TableHead>
            <TableHead>Dietary Restrictions</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {filteredRSVPs.map((rsvp) => (
            <TableRow key={rsvp.id}>
              <TableCell>{rsvp.name}</TableCell>
              <TableCell>{rsvp.email}</TableCell>
              <TableCell>{getStatusBadge(rsvp.status)}</TableCell>
              <TableCell>{rsvp.guests}</TableCell>
              <TableCell>{rsvp.dietaryRestrictions}</TableCell>
              <TableCell>
                <Button variant='ghost' size='sm'>
                  Edit
                </Button>
                <Button variant='ghost' size='sm'>
                  Delete
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}
